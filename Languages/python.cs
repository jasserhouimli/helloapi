using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Languages
{
    public class Python : ControllerBase
    {
        public IActionResult Run(string source, ProblemStruct problem)
        {
            string randomFileName = Path.GetRandomFileName();
            string currentDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(currentDirectory, "tmp");

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            string sourceFile = Path.Combine(tempDirectory, $"{randomFileName}.py");

            try
            {
                System.IO.File.WriteAllText(sourceFile, source);
                Process runProcess = new Process();
                runProcess.StartInfo.FileName = "python"; 
                runProcess.StartInfo.Arguments = sourceFile;
                runProcess.StartInfo.RedirectStandardInput = true;
                runProcess.StartInfo.RedirectStandardOutput = true;
                runProcess.StartInfo.RedirectStandardError = true;
                runProcess.Start();

                int testCaseIndex = 0;
                foreach (var testcase in problem.TestCases)
                {
                    testCaseIndex++;

                    using (var writer = runProcess.StandardInput)
                    {
                        // Input the test case to the program
                        writer.WriteLine(testcase.input);
                    }

                    string output = runProcess.StandardOutput.ReadToEnd().Trim();
                    runProcess.WaitForExit();

                    // Compare the output to the expected output
                    if (output != testcase.expectedOutput.Trim())
                    {
                        return BadRequest($"Test case {testCaseIndex} failed. Expected: {testcase.expectedOutput}, Got: {output}");
                    }
                }

                return Ok("All test cases passed.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest($"File access error: {ex.Message}");
            }
            catch (IOException ex)
            {
                return BadRequest($"File system error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(sourceFile))
                {
                    System.IO.File.Delete(sourceFile);
                }
            }
        }
    }
}
