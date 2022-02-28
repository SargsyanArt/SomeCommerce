using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SomeCommerce.Core.Entities;
using SomeCommerce.DAL.Data;

namespace SomeCommerce.DAL
{
    public static class Seed
    {
        //can possibly come from some config file (Appsettings.Development.json)
        //is hardcoded just for simplicity
        private const string DefaultUsername = "vladica@ognjanovic.com";
        private const string PasswordPlainText = "password";

        public static async void EnsureSeedData(IServiceProvider services)
        {
            using IServiceScope scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using UserManager<SomeUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<SomeUser>>();
            using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context != null)
            {
                // migrate any database changes on startup (includes initial db creation)
                context.Database.Migrate();

                if ((await _userManager.FindByNameAsync(DefaultUsername)) is null)
                {
                    SomeUser adminUser = new()
                    {
                        UserName = DefaultUsername,
                        Email = DefaultUsername,
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(adminUser, PasswordPlainText);
                }

                if (!await context.ProductGroups.AnyAsync())
                {
                    List<ProductGroup> groups = new()
                    {
                        new()
                        {
                            Active = true,
                            Description = "Electronics"
                        },
                        new()
                        {
                            Active = true,
                            Description = "Pharmacy"
                        },
                        new()
                        {
                            Active = true,
                            Description = "Fresh"
                        },
                        new()
                        {
                            Active = true,
                            Description = "Grocery"
                        },
                        new()
                        {
                            Active = true,
                            Description = "Car Parts"
                        }
                    };
                    context.ProductGroups.AddRange(groups);
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    int firstGroupId = (await context.ProductGroups.FirstAsync()).Id;

                    List<Product> products = new()
                    {
                        new()
                        {
                            Active = true,
                            Description = "Mobile Phone",
                            Price = 12.99m,
                            ProductGroupId = firstGroupId
                        },
                        new()
                        {
                            Active = true,
                            Description = "Wireless Charger",
                            Price = 10.99m,
                            ProductGroupId = firstGroupId
                        },
                        new()
                        {
                            Active = true,
                            Description = "Power Bank",
                            Price = 0.98m,
                            ProductGroupId = firstGroupId
                        },
                        new()
                        {
                            Active = true,
                            Description = "USB Cable",
                            Price = 0.98m,
                            ProductGroupId = firstGroupId
                        }
                    };
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }

            }
        }
    }
}
