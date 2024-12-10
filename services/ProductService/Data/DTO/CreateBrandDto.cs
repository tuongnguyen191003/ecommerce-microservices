public class CreateBrandDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public bool IsActive { get; set; } = true;
}
