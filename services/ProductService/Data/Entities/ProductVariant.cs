public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string Color { get; set; }
    public string Storage { get; set; } // Dung lượng
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public bool IsAvailable { get; set; } = true;
    public ICollection<ProductVariantDiscount> ProductVariantDiscounts { get; set; }
}
