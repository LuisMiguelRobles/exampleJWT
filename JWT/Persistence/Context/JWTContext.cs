using JWT.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT.Connection
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
    }
}