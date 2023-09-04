

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PTI.Models;
using PTI.Domain;

namespace PTI.Data
{
    public class AppDbContext : IdentityDbContext <ApplicationUser, IdentityRole, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Product> Products { get; set; }
    }
}
