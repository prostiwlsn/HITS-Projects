using Microsoft.EntityFrameworkCore;
using Profile.Data.Models;

namespace Profile.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }
        public DbSet<ProfileInformation> Profiles { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<EducationDocument> EducationDocuments { get; set; }
        public DbSet<DocumentFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().UseTptMappingStrategy();
        }
    }
}
