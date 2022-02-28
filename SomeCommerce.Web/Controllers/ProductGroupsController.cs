using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeCommerce.Core.Entities;
using SomeCommerce.DAL.Data;
using SomeCommerce.Web.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeCommerce.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/")]
    public class ProductGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductGroups
        [Route("/[controller]/")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductGroups.ToListAsync());
        }

        // GET: ProductGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // GET: ProductGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,Active")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productGroup);
        }

        // GET: ProductGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _context.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return NotFound();
            }
            return View(productGroup);
        }

        // POST: ProductGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,Active")] ProductGroup productGroup)
        {
            if (id != productGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductGroupExists(productGroup.Id))
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
            return View(productGroup);
        }

        // GET: ProductGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productGroup = await _context.ProductGroups.FindAsync(id);
            _context.ProductGroups.Remove(productGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductGroupExists(int id)
        {
            return _context.ProductGroups.Any(e => e.Id == id);
        }

        [HttpPost("/[controller]/[action]")]
        public async Task<JsonResult> GetSelectBox(string term)
        {
            term = term?.ToLower();
            List<Dropdown.Option> productGroups = await _context.ProductGroups
                .Where(p => string.IsNullOrEmpty(term) || p.Description.StartsWith(term))
                .Take(Dropdown.DefaultCapacity)
                .Select(p => new Dropdown.Option(p.Id.ToString(), p.Description))
                .ToListAsync();

            return Json(productGroups);
        }
    }
}
