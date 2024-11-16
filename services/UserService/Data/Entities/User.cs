namespace UserService.Entities
{
    public class User
    {
        public int Id { get; set; } // Primary Key
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
