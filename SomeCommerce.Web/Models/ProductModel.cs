using SomeCommerce.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SomeCommerce.Web.Models
{
    public class ProductModel
{
        public int Id { get; set; }

        [Display(Name = "Product Group")]
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public string Description { get; set; }

        [Display(Name = "Product Number")]
        public Guid Number { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
        public bool Active { get; set; }
    }
}
