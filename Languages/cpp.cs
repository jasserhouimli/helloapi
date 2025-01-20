using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Languages
{
    public class Cpp : ControllerBase
    {
        private readonly AppDbContext _context;

        public Cpp(AppDbContext context)
        {
            _context = context;
        }

        

        // Method to preprocess the C++ source code to remove #ifdef ONLINE_JUDGE and #ifndef ONLINE_JUDGE blocks
        private string PreprocessSourceCode(string sourceCode)
        {
            // Regular expression to match and remove code inside #ifdef ONLINE_JUDGE ... #endif and #ifndef ONLINE_JUDGE ... #endif
            string pattern = @"#\s*(ifdef|ifndef)\s+ONLINE_JUDGE[\s\S]*?#endif"; // Match everything between #ifdef/#ifndef ONLINE_JUDGE and #endif
            return Regex.Replace(sourceCode, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        private void UpdateSubmissionStatus(int submissionId, string status)
        {
            var submission = _context.Submissions.Find(submissionId);
            submission.status = status;
            if(status == "Accepted")
            {
                submission.accepted = true;
            }
            _context.SaveChanges();
        }

        public IActionResult Run(string source, ProblemStruct problem, int submissionId)
        {
            // Preprocess the source code to remove any ONLINE_JUDGE specific code
            string processedSource = PreprocessSourceCode(source);
            
            string randomFileName = Path.GetRandomFileName();
            string currentDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(currentDirectory, "tmp");

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            string sourceFile = Path.Combine(tempDirectory, $"{randomFileName}.cpp");
            string executableFile = Path.Combine(tempDirectory, $"{randomFileName}.out");

            try
            {
                // Write the preprocessed source code to the file
                System.IO.File.WriteAllText(sourceFile, processedSource);

                // Compile the C++ source file
                Process compileProcess = new Process();
                compileProcess.StartInfo.FileName = "g++";
                compileProcess.StartInfo.Arguments = $"{sourceFile} -o {executableFile}";
                compileProcess.StartInfo.RedirectStandardOutput = true;
                compileProcess.StartInfo.RedirectStandardError = true;
                compileProcess.Start();
                compileProcess.WaitForExit();

                // Handle compilation errors
                if (compileProcess.ExitCode != 0)
                {
                    string errorOutput = compileProcess.StandardError.ReadToEnd();
                    UpdateSubmissionStatus(submissionId, "Compilation Error");
                    return new JsonResult(new 
                    { 
                        success = false, 
                        message = $"Compilation failed: {errorOutput}" 
                    })
                    { StatusCode = 200 };
                }

                // Run the compiled executable on each test case
                int c = 0;
                foreach (var testcase in problem.TestCases)
                {

                    //// wait some time
                    ///
                    System.Threading.Thread.Sleep(1000);
                    c++;
                    UpdateSubmissionStatus(submissionId, $"Running test case {c}");
                    Process runProcess = new Process();
                    runProcess.StartInfo.FileName = executableFile;
                    runProcess.StartInfo.RedirectStandardInput = true;
                    runProcess.StartInfo.RedirectStandardOutput = true;
                    runProcess.StartInfo.RedirectStandardError = true;
                    runProcess.Start();

                    using (var writer = runProcess.StandardInput)
                    {
                        writer.WriteLine(testcase.input);
                    }

                    string output = runProcess.StandardOutput.ReadToEnd().Trim();
                    runProcess.WaitForExit();

                    if (output != testcase.expectedOutput.Trim())
                    {
                        UpdateSubmissionStatus(submissionId, $"Wrong answer on test case {c}");
                        return new JsonResult(new 
                        { 
                            success = false, 
                            message = $"Test case {c} failed. Expected: {testcase.expectedOutput}, Got: {output}" 
                        })
                        { StatusCode = 200 };
                    }
                }

                UpdateSubmissionStatus(submissionId, "Accepted");
                return new JsonResult(new 
                { 
                    success = true, 
                    message = "All test cases passed." 
                })
                { StatusCode = 200 };
            }
            catch (UnauthorizedAccessException ex)
            {
                UpdateSubmissionStatus(submissionId, "File access error");
                return new JsonResult(new 
                { 
                    success = false, 
                    message = $"File access error: {ex.Message}" 
                })
                { StatusCode = 200 };
            }
            catch (IOException ex)
            {
                UpdateSubmissionStatus(submissionId, "File system error");
                return new JsonResult(new 
                { 
                    success = false, 
                    message = $"File system error: {ex.Message}" 
                })
                { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                UpdateSubmissionStatus(submissionId, "An error occurred");
                return new JsonResult(new 
                { 
                    success = false, 
                    message = $"An error occurred: {ex.Message}" 
                })
                { StatusCode = 500 };
            }
            finally
            {
                // Clean up the temporary files
                if (System.IO.File.Exists(sourceFile))
                {
                    System.IO.File.Delete(sourceFile);
                }
                if (System.IO.File.Exists(executableFile))
                {
                    System.IO.File.Delete(executableFile);
                }
            }
        }
    }
}
