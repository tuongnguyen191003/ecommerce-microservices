using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SharedLibrary.DependencyInjection;
using SharedLibrary.Middleware;
using SharedLibrary.Security;
using UserService.Data;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build()) // Đọc cấu hình Serilog từ appsettings.json
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(); // Tích hợp Serilog với ứng dụng

Console.WriteLine($"JWT Key from Program.cs: {builder.Configuration["Authentication:Key"]}");

// Đăng ký các dịch vụ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Service API",
        Version = "v1"
    });

    // Cấu hình JWT Bearer Token
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsIn...'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Đăng ký DbContext với connection string
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("eCommerceConnection")));

// Đăng ký SharedServices từ SharedLibrary
builder.Services.AddSharedServices<UserDbContext>(builder.Configuration, "UserServiceLogs");

// Đăng ký JwtTokenGenerator
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddControllers();

var app = builder.Build();

// Gọi SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Gọi SeedData để seed dữ liệu
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while seeding the database.");
    }
}

// Cấu hình middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Bật JWT Authentication
app.UseAuthorization();
app.UseMiddleware<GlobalException>(); // Middleware xử lý lỗi toàn cục
app.UseSharedPolicies(); // Chính sách từ SharedLibrary

app.MapControllers();

app.Run();
