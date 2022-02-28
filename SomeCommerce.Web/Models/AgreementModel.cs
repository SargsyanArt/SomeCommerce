using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace SomeCommerce.Web.Models
{
    public class AgreementModel
    {
        public AgreementModel()
        { }

        public AgreementModel(SelectList allProductGroups, SelectList allProducts, decimal? productPrice)
        {
            AllProductGroups = allProductGroups;
            AllProducts = allProducts;
            ProductPrice = NewPrice = productPrice ?? default;
        }

        public int Id { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }
        public SomeUserModel User { get; set; }

        [Display(Name = "Product")]
        [Required]
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public SelectList AllProducts { get; set; }
        
        [Required]
        [Display(Name = "Product Group")]
        public int? ProductGroupId { get; set; }
        public ProductGroupModel ProductGroup { get; set; }
        public SelectList AllProductGroups { get; set; }


        [Display(Name = "Effective Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMonths(1);

        [Display(Name = "Product Price")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "New Price")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal NewPrice { get; set; }
    }
}
