namespace ProductService.Data.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OptionName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();
    }
}
