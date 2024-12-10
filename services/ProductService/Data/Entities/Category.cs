public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool Published { get; set; } = false;  
    public int DisplayOrder { get; set; } 
    public Category ParentCategory { get; set; }  
    public List<Category> ChildCategories { get; set; } 
    
    public ICollection<Product> Products { get; set; }
}
