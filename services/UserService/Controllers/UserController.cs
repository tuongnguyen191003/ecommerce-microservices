using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Security;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly JwtTokenGenerator _tokenGenerator;

        public UserController(JwtTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return BadRequest(new { message = "UserId is required." });
            }

            var token = _tokenGenerator.GenerateToken(request.UserId);
            return Ok(new { Token = token });
        }
    }
}
