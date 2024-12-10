public class ProductVariantDiscount
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; }
    public int DiscountId { get; set; }
    public Discount Discount { get; set; }
    public decimal PriceAfterDiscount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}
