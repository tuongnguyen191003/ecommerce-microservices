using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace SharedLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection services, IConfiguration config) {
            // Log các giá trị cấu hình để kiểm tra
            Console.WriteLine($"JWT Key: {config["Authentication:Key"]}");
            Console.WriteLine($"JWT Issuer: {config["Authentication:Issuer"]}");
            Console.WriteLine($"JWT Audience: {config["Authentication:Audience"]}");

            var key = config["Authentication:Key"];
            var issuer = config["Authentication:Issuer"];
            var audience = config["Authentication:Audience"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException("JWT configuration values cannot be null or empty.");
            }
            
            //add JWT service 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    // var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                    // string issuer = config.GetSection("Authentication:Issuer").Value!;
                    // string audience = config.GetSection("Authentication:Audience").Value!;

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                        // IssuerSigningKey = new SymmetricSecurityKey(key)
                        //ValidIssuer = config["Authentication:Issuer"],
                        //ValidAudience = config["Authentication:Audience"],
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:Key"]))
                    };
                });
            return services;
        }
    }
}