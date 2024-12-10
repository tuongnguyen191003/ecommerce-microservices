using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace SharedLibrary.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string userId, IEnumerable<string> roles)
        {
            try
            {
                var key = _config["Authentication:Key"];
                var issuer = _config["Authentication:Issuer"];
                var audience = _config["Authentication:Audience"];

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    Log.Error($"Invalid configuration: Key={key}, Issuer={issuer}, Audience={audience}");
                    throw new Exception("JWT configuration is invalid.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Thêm các claims cơ bản và roles vào token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Thêm roles vào claims
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Log.Error($"Token generation failed: {ex.Message}");
                throw;
            }
        }
    }
}
