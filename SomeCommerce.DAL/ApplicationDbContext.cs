using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SomeCommerce.Core.Entities;

namespace SomeCommerce.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<SomeUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Performance Diagnosis https://docs.microsoft.com/en-us/ef/core/performance/performance-diagnosis?tabs=simple-logging%2Cload-entities
            //optionsBuilder
            //    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            //    .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(p => p.Number)
                .HasDefaultValueSql("NEWID()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<Product>().HasIndex(p => p.Number).IsUnique();
            builder.Entity<Product>().HasIndex(p => p.Description).IsClustered(false);

            builder.Entity<ProductGroup>().Property(pg => pg.Code)
                .HasDefaultValueSql("NEWID()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<ProductGroup>().HasIndex(pg => pg.Code).IsUnique();
            builder.Entity<ProductGroup>().HasIndex(pg => pg.Description).IsClustered(false);

            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
    }
}
