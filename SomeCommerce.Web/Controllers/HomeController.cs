using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomeCommerce.Core.Entities;
using SomeCommerce.DAL.Data;
using SomeCommerce.Web.Configuration;
using SomeCommerce.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeCommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _dbContext.Products
                .Where(p => p.Active)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Description = p.Description,
                    Price = p.Price,
                })
                .Take(Dropdown.DefaultCapacity)
                .ToListAsync();

            SelectList allProducts = new(products, nameof(Product.Id), nameof(Product.Description));
            
            
            List<ProductGroup> productGroups = await _dbContext.ProductGroups
                .Where(p => p.Active)
                .Select(p => new ProductGroup
                {
                    Id = p.Id,
                    Description = p.Description,
                })
                .Take(Dropdown.DefaultCapacity)
                .ToListAsync();

            SelectList allProductGroups = new(productGroups, nameof(ProductGroup.Id), nameof(ProductGroup.Description));
            AgreementModel agreement = new(allProductGroups, allProducts, products.FirstOrDefault()?.Price);
            IndexModel model = new(agreement);
            return View(model);
        }
    }
}
