using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using System.Diagnostics;
using System.IO;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProblemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProblemStruct>>> GetProblems()
        {
            return await _context.Problems.Include(p => p.TestCases).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProblemStruct>> GetProblem(int id)
        {
            var problemStruct = await _context.Problems
                .Include(p => p.TestCases)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (problemStruct == null)
            {
                return NotFound();
            }

            return problemStruct;
        }

        [HttpPost]
        public async Task<ActionResult<ProblemStruct>> PostProblem(ProblemStruct problem)
        {
            _context.Problems.Add(problem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProblem", new { id = problem.Id }, problem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProblemStruct>> DeleteProblem(int id)
        {
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();

            return problem;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllProblems()
        {
            var problems = await _context.Problems.ToListAsync();
            if (problems == null)
            {
                return NotFound();
            }

            _context.Problems.RemoveRange(problems);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("user/submissions/{id}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForUser(int id)
        {
            var submissions = await _context.Submissions
                .Where(s => s.userId == id).OrderByDescending(s => s.submissionId)
                .ToListAsync();

            return Ok(submissions);
        }

        [HttpGet("problem/submissions/{id}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForProblem(int id)
        {
            var submissions = await _context.Submissions
                .Where(s => s.problemId == id)
                .ToListAsync();

            return Ok(submissions);
        }

        [HttpGet("problem/submissions/{id}/user/{userId}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForProblemByUser(int id, int userId)
        {
            var submissions = await _context.Submissions
                .Where(s => s.problemId == id && s.userId == userId)
                .OrderByDescending(s => s.submissionId)
                .ToListAsync();

            return Ok(submissions);
        }


        
    }
}
