using Microsoft.EntityFrameworkCore;
using Personell.Data.Models;

namespace Personell.Data
{
    public class PersonellDbContext : DbContext
    {
        public DbSet<Models.Personell> Personell {  get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<MainManager> MainManagers { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Application> Applications { get; set; }

        public PersonellDbContext(DbContextOptions<PersonellDbContext> options) : base(options) { }
    }
}