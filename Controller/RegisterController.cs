using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {

        private readonly AppDbContext _context;

        public RegisterController(AppDbContext context)
        {
            _context = context;
        }
        

        // POST api/register

        [HttpPost]

        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User must be provided");
            }

            if (user.Username == null || user.Password == null || user.Email == null)
            {
                return BadRequest("Username, password and email must be provided");
            }

            if (user.Username.Length < 3 || user.Password.Length < 3 || user.Email.Length < 3)
            {
                return BadRequest("Username, password and email must be at least 3 characters long");
            }

            if (user.isAdmin)
            {
                return BadRequest("You cannot register as an admin");
            }

            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userExists != null)
            {
                return BadRequest("User already exists");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}