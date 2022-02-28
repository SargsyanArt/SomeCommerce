using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomeCommerce.Core.Entities;

namespace SomeCommerce.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<SomeUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Agreement>().HasOne(a => a.ProductGroup).WithMany(pg => pg.Agreements).OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<Product>().HasIndex(s => s.Description).IsClustered();
            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Agreement> Aggreements { get; set; }
    }
}
