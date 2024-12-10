public class CreateCategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Published { get; set; } = false;
    public int DisplayOrder { get; set; }
    public string Slug { get; set; }
}
