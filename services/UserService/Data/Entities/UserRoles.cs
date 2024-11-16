namespace UserService.Entities
{
    public class UserRole
    {
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key to Users
        public int RoleId { get; set; } // Foreign Key to Roles
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
