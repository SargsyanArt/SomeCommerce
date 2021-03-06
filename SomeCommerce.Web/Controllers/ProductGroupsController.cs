using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductGroupsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Route("/[controller]/")]
        public IActionResult Index() => View();

        public async Task<JsonResult> Get(DataTableAjaxPostModel model)
        {
            IQueryable<ProductGroup> query = _context.ProductGroups
                        .Where(pg => model.search == null || string.IsNullOrEmpty(model.search.value) || pg.Description.StartsWith(model.search.value));

            List<ProductGroupModel> productGroups = await query
                        .Skip(model.start)
                        .Take(model.length)
                        .ProjectTo<ProductGroupModel>(_mapper.ConfigurationProvider)
                        .ToListAsync();

            return Json(new
            {
                // this is what datatables wants sending back
                model.draw,
                recordsTotal = productGroups.Count,
                recordsFiltered = await query.CountAsync(),
                data = productGroups.Select(a => new
                {
                    id = a.Id,
                    description = a.Description,
                    code = a.Code,
                    active = a.Active
                })
            });
        }

        // GET: ProductGroups/Details/5
        [Route("{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductGroup productGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductGroupModel>(productGroup));
        }

        // GET: ProductGroups/Create
        public IActionResult Create() => View();

        // POST: ProductGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,Active")] ProductGroupModel productGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_mapper.Map<ProductGroup>(productGroup));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productGroup);
        }

        [Route("{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductGroup productGroup = await _context.ProductGroups.FindAsync(id);
            if (productGroup == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<ProductGroupModel>(productGroup));
        }

        // POST: ProductGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,Active")] ProductGroupModel productGroup)
        {
            if (id != productGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_mapper.Map<ProductGroup>(productGroup));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductGroupExists(productGroup.Id))
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

        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductGroup productGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductGroupModel>(productGroup));
        }

        [HttpPost("{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ProductGroup productGroup = await _context.ProductGroups.FindAsync(id);
            _context.ProductGroups.Remove(productGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductGroupExists(int id) => await  _context.ProductGroups.AnyAsync(e => e.Id == id);

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
