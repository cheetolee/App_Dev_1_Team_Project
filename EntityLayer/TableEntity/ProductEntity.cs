using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    [Table("Products")]
    public class ProductEntity 
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

    }
}