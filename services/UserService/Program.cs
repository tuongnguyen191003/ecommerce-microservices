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
builder.Services.AddSwaggerGen();

// Đăng ký DbContext với connection string
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("eCommerceConnection")));

// Đăng ký SharedServices từ SharedLibrary
builder.Services.AddSharedServices<UserDbContext>(builder.Configuration, "UserServiceLogs");

// Đăng ký JwtTokenGenerator
builder.Services.AddSingleton<JwtTokenGenerator>();


builder.Services.AddControllers();

var app = builder.Build();

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
