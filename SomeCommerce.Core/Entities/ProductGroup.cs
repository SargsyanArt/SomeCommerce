using System.ComponentModel.DataAnnotations.Schema;

namespace SomeCommerce.Core.Entities
{
    public class ProductGroup
    {
        public int Id { get; set; }
        public string Description { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid Code { get; set; }
        public bool Active { get; set; }

        public List<Product> Products { get; set; }
        public List<Agreement> Agreements { get; set; }
    }
}
