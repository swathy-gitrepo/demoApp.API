using DemoApp.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create a unique index on FirstName, LastName, and PhoneNumber
            modelBuilder.Entity<Contact>()
                .HasIndex(c => new { c.FirstName, c.LastName, c.PhoneNumber })
                .IsUnique()
                .HasDatabaseName("IX_unique_contacts");
        }
    }
}
