using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = new UserDbContext(serviceProvider.GetRequiredService<DbContextOptions<UserDbContext>>());

        // Seed Roles
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin", Description = "Administrator role with full permissions" },
                new Role { Name = "User", Description = "Regular user role" }
            );
            await context.SaveChangesAsync();
        }

        // Seed User
        if (!context.Users.Any(u => u.Username == "TuongAdmin"))
        {
            var admin = new User
            {
                Username = "TuongAdmin",
                Password = BCrypt.Net.BCrypt.HashPassword("Tuong191003"),
                Email = "nguyenminhtuong19102003@gmail.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(admin);
            await context.SaveChangesAsync();

            // Assign Admin Role
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole != null)
            {
                context.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleId = adminRole.Id,
                    AssignedAt = DateTime.UtcNow
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
