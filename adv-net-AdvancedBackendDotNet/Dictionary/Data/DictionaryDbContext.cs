using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Data
{
    public class DictionaryDbContext : DbContext
    {
        public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options) : base(options) { }

        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<EducationProgram> Programs { get; set; }

        public DbSet<UpdateState> UpdateStates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentType>()
                .HasOne(d => d.EducationLevel)
                .WithMany()
                .HasForeignKey(d => d.EducationLevelId)
                .IsRequired();

            modelBuilder.Entity<DocumentType>()
                .HasMany(d => d.NextEducationLevels)
                .WithMany(e => e.DocumentTypes)
                .UsingEntity(j => j.ToTable("DocumentTypeEducationLevel"));

            modelBuilder.Entity<EducationLevel>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
}
