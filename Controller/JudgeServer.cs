using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Languages;
using Microsoft.AspNetCore.Authorization;
namespace api.Controller
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class JudgeServer : ControllerBase
    {
        private readonly AppDbContext _context;

        public JudgeServer(AppDbContext context)
        {
            _context = context;
        }

        

        // api/run/problem/1
        [HttpPost("run/problem/{id}")]
        public async Task<IActionResult> RunCode(int id , int userID, [FromBody] Code code)
        {
            var problem = await _context.Problems
                    .Include(p => p.TestCases) 
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (problem == null)
            {
                return NotFound("Problem not found.");
            }

            string language = code.language;
            string source = code.source;


            Submission submission = new Submission
            {
                problemId = id,
                userId = userID,
                source = source,
                language = language,
                status = "Running"
            };

            _context.Submissions.Add(submission);

            await _context.SaveChangesAsync();

            int lastId = submission.submissionId;

            switch (language)
            {
                case "python":
                    var pythonRunner = new Python();
                    return pythonRunner.Run(source, problem);
                case "cpp":
                    var cppRunner = new Cpp(_context);
                    return cppRunner.Run(source, problem , lastId);
                case "java":
                    var javaRunner = new Java();
                    return javaRunner.Run(source, problem);
                case "c":
                    var cRunner = new C();
                    return cRunner.Run(source, problem);                
                default:
                    return BadRequest("Language not supported");
            }
        }




    
        
    }
}
