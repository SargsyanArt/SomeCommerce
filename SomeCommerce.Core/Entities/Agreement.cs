using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomeCommerce.Core.Entities
{
    public class Agreement
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public SomeUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int? ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal ProductPrice { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal NewPrice { get; set; }
    }
}
