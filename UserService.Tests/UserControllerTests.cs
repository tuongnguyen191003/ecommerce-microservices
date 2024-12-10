using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Controllers;
using UserService.Data;
using UserService.Entities;
using UserService.Models;
using UserService.Tests.TestHelpers;
using SharedLibrary.Security;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
namespace UserService.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly UserDbContext _dbContext;
        private readonly Mock<IJwtTokenGenerator> _mockTokenGenerator;

        public UserControllerTests()
        {
            _dbContext = TestDbContextFactory.CreateInMemoryDbContext();
            _mockTokenGenerator = new Mock<IJwtTokenGenerator>();
            _controller = new UserController(_mockTokenGenerator.Object, _dbContext);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenUserIsRegisteredSuccessfully()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "TestUser",
                Password = "TestPassword",
                Email = "test@example.com"
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<RegisterResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("User registered successfully.", response.Message);
        }

        [Fact]
        public async Task Register_ReturnsConflictResult_WhenUsernameAlreadyExists()
        {
            // Arrange
            var user = new User
            {
                Username = "ExistingUser",
                Password = "PasswordHash",
                Email = "existing@example.com"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var request = new RegisterRequest
            {
                Username = "ExistingUser",
                Password = "NewPassword",
                Email = "new@example.com"
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<RegisterResponse>(conflictResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Username or Email already exists.", response.Message);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                Username = "ValidUser",
                Password = HashPassword("ValidPassword"),
                Email = "valid@example.com"
            };

            _dbContext.Users.Add(user);
            _dbContext.Roles.Add(new Role { Id = 1, Name = "Admin" }); // Thêm role
            _dbContext.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = 1 });
            await _dbContext.SaveChangesAsync();

            // Mock JwtTokenGenerator để trả về token giả lập
            var roles = new List<string> { "Admin" };
            _mockTokenGenerator
                .Setup(x => x.GenerateToken(It.IsAny<string>(), roles))
                .Returns("FakeJWTToken");

            var request = new LoginRequest
            {
                Username = "ValidUser",
                Password = "ValidPassword" // Mật khẩu gốc chưa hash
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.NotNull(response);
            Assert.Equal("FakeJWTToken", response.Token);
        }

        // Hàm hash giống trong UserController
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "InvalidUser",
                Password = "InvalidPassword"
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        //----------------------------------------------------------------
        //Kiểm thử UpdateProfile
        // Kiểm thử thành công (200 OK)
        [Fact]
        public async Task GetProfile_ReturnsOkResult_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new User
            {
                Username = "ValidUser",
                Password = HashPassword("ValidPassword"),
                Email = "valid@example.com",
                PhoneNumber = "123456789"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("ValidUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await _controller.GetProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UserProfileResponse>(okResult.Value);
            Assert.Equal("ValidUser", response.Username);
            Assert.Equal("valid@example.com", response.Email);
            Assert.Equal("123456789", response.PhoneNumber);
        }

        //Kiểm thử khi user không tồn tại (404 Not Found)
        [Fact]
        public async Task GetProfile_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("NonExistentUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await _controller.GetProfile();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        //----------------------------------------------------------------
        // Put Profile
        //Kiểm thử thành công (200 OK)
        [Fact]
        public async Task UpdateProfile_ReturnsUpdatedProfile_WhenProfileIsUpdatedSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "ValidUser",
                Password = HashPassword("ValidPassword"),
                Email = "old@example.com",
                PhoneNumber = "123456789"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("ValidUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            var request = new UpdateProfileRequest
            {
                Email = "new@example.com",
                PhoneNumber = "987654321"
            };

            // Act
            var result = await _controller.UpdateProfile(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UserProfileResponse>(okResult.Value);
            Assert.Equal("ValidUser", response.Username);
            Assert.Equal("new@example.com", response.Email);
            Assert.Equal("987654321", response.PhoneNumber);

            // Verify changes in database
            var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "ValidUser");
            Assert.NotNull(updatedUser);
            Assert.Equal("new@example.com", updatedUser!.Email);
            Assert.Equal("987654321", updatedUser.PhoneNumber);
        }

        //Kiểm thử khi user không tồn tại (404 Not Found)
        [Fact]
        public async Task UpdateProfile_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("NonExistentUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            var request = new UpdateProfileRequest
            {
                Email = "new@example.com",
                PhoneNumber = "987654321"
            };

            // Act
            var result = await _controller.UpdateProfile(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        //----------------------------------------------------------------
        // Delete Profile
        //Kiểm thử thành công (200 OK)
        [Fact]
        public async Task DeleteProfile_ReturnsOkResult_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "ValidUser",
                Password = HashPassword("ValidPassword"),
                Email = "valid@example.com"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("ValidUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await _controller.DeleteProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<DeleteProfileResponse>(okResult.Value);
            Assert.Equal("Profile deleted successfully.", response.Message);

            // Verify user is removed from database
            var deletedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "ValidUser");
            Assert.Null(deletedUser);
        }

        //Kiểm thử khi user không tồn tại (404 Not Found)
        [Fact]
        public async Task DeleteProfile_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User.Identity!.Name).Returns("NonExistentUser");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await _controller.DeleteProfile();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
