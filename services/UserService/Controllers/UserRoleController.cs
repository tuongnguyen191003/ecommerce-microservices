using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/admin/userroles")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới có thể gán role
    public class UserRoleController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserRoleController(UserDbContext context)
        {
            _context = context;
        }

        // GET api/admin/userroles
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return Ok(new
            {
                UserId = user.Id,
                Username = user.Username,
                Roles = roles
            });
        }

        // POST: /api/admin/userroles
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found" });
            }

            // Check if the user already has this role
            if (user.UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                return BadRequest(new { Message = "User already has this role" });
            }

            var userRole = new UserRole
            {
                UserId = request.UserId,
                RoleId = role.Id,
                AssignedAt = DateTime.UtcNow
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Role '{role.Name}' assigned to user '{user.Username}' successfully." });
        }

        // DELETE: /api/admin/userroles/{userId}/{roleName}
        [HttpDelete("{userId}/{roleName}")]
        public async Task<IActionResult> RemoveRole(int userId, string roleName)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);

            if (userRole == null)
            {
                return NotFound(new { Message = "User role not found" });
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Role '{roleName}' removed from user successfully." });
        }
    }
}
