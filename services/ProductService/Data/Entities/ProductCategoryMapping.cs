namespace ProductService.Data.Entities
{
    public class ProductCategoryMapping
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
