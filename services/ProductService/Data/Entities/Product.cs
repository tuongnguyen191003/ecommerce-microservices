public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public int? CategoryId { get; set; }
    public Category Category { get; set; }
    public int? BrandId { get; set; }
    public Brand Brand { get; set; }
    public bool IsPublished { get; set; } = false;
    public string Tags { get; set; }
    public bool IsShowHomePage { get; set; } = false;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public bool IsNew { get; set; } = false;
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public decimal CostPrice { get; set; }
    public bool IsBuyButton { get; set; } = false;
    public bool IsWishList { get; set; } = false;
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string MetaKeywords { get; set; }
    public string SeoUrl { get; set; }
    public string Picture { get; set; }
    public string Video { get; set; }


    public ICollection<ProductVariant> ProductVariants { get; set; }
    public ICollection<ProductDiscount> ProductDiscounts { get; set; }
}
