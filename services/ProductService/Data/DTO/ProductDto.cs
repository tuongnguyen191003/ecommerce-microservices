public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public bool IsPublished { get; set; }
    public string Tags { get; set; }
    public bool IsShowHomePage { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public decimal CostPrice { get; set; }
    public bool IsBuyButton { get; set; }
    public bool IsWishList { get; set; }
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string MetaKeywords { get; set; }
    public string SeoUrl { get; set; }
    public string Picture { get; set; }
    public string Video { get; set; }
    public string Slug { get; set; }
}
