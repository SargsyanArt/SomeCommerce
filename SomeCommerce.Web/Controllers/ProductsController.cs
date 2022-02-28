using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("[controller]/[action]/")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        // GET: Products1
        [Route("/[controller]/")]
        public IActionResult Index() => View();

        public async Task<JsonResult> Get(DataTableAjaxPostModel model)
        {
            IQueryable<Product> query = _context.Products
                        .Where(pg => model.search == null || string.IsNullOrEmpty(model.search.value) || pg.Description.StartsWith(model.search.value));

            List<Product> products = await query
                        .Skip(model.start)
                        .Take(model.length)
                        .Select(p => new Product { 
                            Id = p.Id,
                            Active = p.Active,
                            Description = p.Description,
                            Number = p.Number,
                            Price = p.Price,
                            ProductGroupId = p.ProductGroupId,
                            ProductGroup = new ProductGroup { 
                                Description = p.ProductGroup.Description,
                            }
                        })
                        .ToListAsync();

            return Json(new
            {
                // this is what datatables wants sending back
                model.draw,
                recordsTotal = products.Count,
                recordsFiltered = await query.CountAsync(),
                data = products.Select(p => new
                {
                    id = p.Id,
                    description = p.Description,
                    productGroup = p.ProductGroup.Description,
                    number = p.Number,
                    price = p.Price.ToString("C2"),
                    active = p.Active
                })
            });
        }

        [Route("{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductModel>(product));
        }

        // GET: Products1/Create
        public IActionResult Create() => View();

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductGroupId,Description,Number,Price,Active")] ProductModel product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_mapper.Map<Product>(product));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (product.ProductGroupId != default)
            {
                product.ProductGroup = await _context.ProductGroups.FindAsync(product.ProductGroupId);
                ViewData["ProductGroupId"] = new SelectList(
                   new List<SelectListItem>{
                        new SelectListItem(product.ProductGroup.Description, product.ProductGroupId.ToString())
                   }, "Value", "Text");
            }
            return View(product);
        }

        [Route("{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new Product { 
                    Id = p.Id,
                    Description = p.Description,
                    Active = p.Active,
                    Number = p.Number,
                    Price = p.Price,
                    ProductGroupId = p.ProductGroupId,
                    ProductGroup = new ProductGroup { 
                        Description = p.ProductGroup.Description,
                    }
                })
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProductGroupId"] = new SelectList(
                new List<SelectListItem>{
                    new SelectListItem(product.ProductGroup.Description, product.ProductGroupId.ToString())
                }, "Value", "Text");
            return View(_mapper.Map<ProductModel>(product));
        }

        // POST: Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductGroupId,Description,Number,Price,Active")] ProductModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_mapper.Map<Product>(product));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            if (product.ProductGroupId != default)
            {
                product.ProductGroup = await _context.ProductGroups.FindAsync(product.ProductGroupId);
                ViewData["ProductGroupId"] = new SelectList(
                   new List<SelectListItem>{
                        new SelectListItem(product.ProductGroup.Description, product.ProductGroupId.ToString())
                   }, "Value", "Text");
            }
            return View(product);
        }

        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductModel>(product));
        }

        [HttpPost("{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(int id) => await _context.Products.AnyAsync(e => e.Id == id);

        [HttpGet("/[controller]/[action]/{productId:int}")]
        public async Task<ProductModel> GetProductDetails(int productId)
        {
            Product product = await _context.Products.FindAsync(productId);
            return _mapper.Map<ProductModel>(product);
        }

        [HttpGet("/[controller]/[action]/{productId:int}")]
        public async Task<decimal> GetProductPrice(int productId)
        {
            Product product = await _context.Products.FindAsync(productId);
            return (product.Price);
        }

        [HttpPost("/[controller]/[action]")]
        public async Task<JsonResult> GetSelectBox(string term)
        {
            term = term?.ToLower();
            List<Product> products = await _context.Products
                .Where(p => string.IsNullOrEmpty(term) || p.Description.StartsWith(term))
                .Take(Dropdown.DefaultCapacity)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Description = p.Description,
                    ProductGroup = new ProductGroup
                    {
                        Description = p.ProductGroup.Description,
                    }
                }).ToListAsync();

            List<Dropdown.OptGroup> result = new();
            foreach (IGrouping<string, Product> group in products.GroupBy(p => p.ProductGroup.Description))
            {
                result.Add(new Dropdown.OptGroup(group.Key, group.Select(g => new Dropdown.Option(g.Id.ToString(), g.Description))));
            }
            return Json(result);
        }
    }
}
