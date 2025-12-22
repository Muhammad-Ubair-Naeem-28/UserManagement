using BlazorApp1.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<FormData> FormData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FormData>(entity =>
            {
                entity.HasKey(x => x.Email);
                entity.Property(x => x.PasswordHash).IsRequired();
                entity.Property(x => x.Role).IsRequired().HasDefaultValue("User");
                entity.Ignore(x => x.Password);
            });
        }
    }
}
