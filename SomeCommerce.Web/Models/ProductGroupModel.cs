using System;
using System.ComponentModel.DataAnnotations;

namespace SomeCommerce.Web.Models
{
    public class ProductGroupModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Product Group Code")]
        public Guid Code { get; set; }
        public bool Active { get; set; }
    }
}
