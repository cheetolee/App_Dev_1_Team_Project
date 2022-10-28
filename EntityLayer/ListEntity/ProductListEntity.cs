using System;

namespace EntityLayer
{
    public class ProductListEntity : IComparable<ProductListEntity>
    {
        public ProductListEntity() { }
        public ProductListEntity(ProductEntity p)
            :this()
        {
            Code = p.Code;
            Price = p.Price;
            Quantity = p.Quantity;
            Id = p.Id;
            Name = p.Name;
        }
        public ProductListEntity(ProductEntity p, ProductCategoryEntity c)
            :this(p)
        {
            Category = c.Category;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public int CompareTo(ProductListEntity other)
        {
            var compare1 = Name ?? string.Empty;
            var compare2 = other.Name ?? string.Empty;
            return compare1.CompareTo(compare2);
        }
    }
}
