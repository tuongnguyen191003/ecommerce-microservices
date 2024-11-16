namespace UserService.Entities
{
    public class Role
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
