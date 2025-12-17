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
    }
}
