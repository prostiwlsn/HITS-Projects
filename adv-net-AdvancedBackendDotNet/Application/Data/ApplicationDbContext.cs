using Application.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Models.ApplicationInfo> Applications { get; set; }
        public DbSet<ChosenProgram> ChosenProgram { get; set; }
    }
}
