using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomeCommerce.Core.Entities;
using SomeCommerce.DAL.Data;
using SomeCommerce.Web.Configuration;
using SomeCommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeCommerce.Web.Controllers
{
    [Authorize]
    [Route("/[controller]/[action]/")]
    public class AgreementsController : Controller
    {
        private readonly UserManager<SomeUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AgreementsController(UserManager<SomeUser> userManager, ApplicationDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<JsonResult> Get(DataTableAjaxPostModel model)
        {
            int userId = int.Parse(_userManager.GetUserId(User));

            IQueryable<Agreement> query = _dbContext.Agreements
                        .Where(a => a.UserId == userId
                        && model.search == null || string.IsNullOrEmpty(model.search.value) ? true : a.Product.Description.StartsWith(model.search.value));

            List<AgreementModel> agreements = await query
                        .Skip(model.start)
                        .Take(model.length)
                        .ProjectTo<AgreementModel>(_mapper.ConfigurationProvider)
                        .ToListAsync();

            return Json(new
            {
                // this is what datatables wants sending back
                model.draw,
                recordsTotal = agreements.Count,
                recordsFiltered = await query.CountAsync(),
                data = agreements.Select(a => new
                {
                    agreementId = a.Id,
                    productNumber = a.Product.Number,
                    productDescription = a.Product.Description,
                    productGroupCode = a.Product.ProductGroup.Code,
                    productGroupDescription = a.Product.ProductGroup.Description,
                    user = a.User.UserName,
                    effectiveDate = a.EffectiveDate.ToShortDateString(),
                    expirationDate = a.ExpirationDate.ToShortDateString(),
                    productPrice = a.ProductPrice.ToString("C2"),
                    newPrice = a.NewPrice.ToString("C2")
                })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgreementModel model)
        {
            if (ModelState.IsValid)
            {
                Agreement agreement = _mapper.Map<Agreement>(model);
                agreement.UserId = int.Parse(_userManager.GetUserId(User));
                await _dbContext.AddAsync(agreement);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var count = ModelState.ErrorCount;
            Agreement agreement = await _dbContext.Agreements.Select(a => new Agreement
            {
                Id = a.Id,
                EffectiveDate = a.EffectiveDate,
                ExpirationDate = a.ExpirationDate,
                ProductId = a.ProductId,
                Product = new Product
                {
                    Description = a.Product.Description
                },
                NewPrice = a.NewPrice,
                ProductPrice = a.ProductPrice,
                ProductGroupId = a.ProductGroupId,
                ProductGroup = new ProductGroup
                {
                    Description = a.ProductGroup.Description
                },
                UserId = a.UserId
            }).FirstOrDefaultAsync(a => a.Id == id);
            AgreementModel model = _mapper.Map<AgreementModel>(agreement);

            model.AllProducts = new SelectList(
                new List<SelectListItem>{
                    new SelectListItem(agreement.Product.Description, agreement.ProductId.ToString())
                }, "Value", "Text");

            model.AllProductGroups = new SelectList(
                new List<SelectListItem>{
                    new SelectListItem(agreement.ProductGroup.Description, agreement.ProductGroupId.ToString())
                }, "Value", "Text");

            if (TempData["ErrorText"] != null)
                ModelState.AddModelError(string.Empty, TempData["ErrorText"].ToString());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(AgreementModel model)
        {
            if (ModelState.IsValid) 
            {
                Agreement agreement = _mapper.Map<Agreement>(model);
                agreement.UserId = int.Parse(_userManager.GetUserId(User));
                _dbContext.Update(agreement);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorText"] = "Unexpected error happened!";
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }
        }

        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Agreement agreement = await _dbContext.Agreements.FindAsync(id);
            if(agreement != null)
            {
                _dbContext.Remove(agreement);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
