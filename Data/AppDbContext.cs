using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProblemStruct> Problems { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and other configurations here if needed.
            
            modelBuilder.Entity<ProblemStruct>()
                .HasMany(p => p.TestCases)
                .WithOne()
                .HasForeignKey(tc => tc.Id); // Assuming TestCase doesn't have a back reference to ProblemStruct.

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.ProblemStruct)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.problemId);
            
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.userId);
        }
    }
}
