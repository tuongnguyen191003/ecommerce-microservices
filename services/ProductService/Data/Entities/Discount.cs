public class Discount
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DiscountType DiscountType { get; set; } // Loại giảm giá: Fixed hoặc Percentage
    public decimal DiscountValue { get; set; } // Giá trị giảm (số tiền hoặc tỷ lệ phần trăm)
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal? MinPurchaseAmount { get; set; } // Số tiền tối thiểu để áp dụng giảm giá
    public decimal? MaxDiscountAmount { get; set; } // Giảm giá tối đa
    public ICollection<ProductDiscount> ProductDiscounts { get; set; }
}

public enum DiscountType
{
    Fixed,       // Giảm giá cố định
    Percentage   // Giảm giá theo tỷ lệ phần trăm
}
