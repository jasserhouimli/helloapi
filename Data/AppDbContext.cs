using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;
namespace api.Data
{
    [Route("[controller]")]
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<ProblemStruct> Problems { get; set; }
        public required DbSet<Code> Codes { get; set; }

        public required DbSet<Submission> Submissions { get; set; }
        public required DbSet<User> Users { get; set; }
        
    }
}