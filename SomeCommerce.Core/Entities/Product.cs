using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomeCommerce.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product Group")]
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public string Description { get; set; }
        public Guid Number { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }
        public bool Active { get; set; }

        public List<Agreement> Agreements { get; set; }
    }
}
