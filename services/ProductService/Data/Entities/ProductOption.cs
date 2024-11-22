namespace ProductService.Data.Entities
{
    public class ProductOption
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        // Navigation properties
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
