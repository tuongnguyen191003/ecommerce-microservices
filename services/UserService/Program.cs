using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLibrary.DependencyInjection;
using SharedLibrary.Middleware;
using SharedLibrary.Security;
using UserService.Data;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/UserServiceLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"JWT Key from Program.cs: {builder.Configuration["Authentication:Key"]}");


// Đăng ký các dịch vụ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Đăng ký các dịch vụ từ SharedLibrary (bao gồm DbContext, JWT, Serilog)
// Add DbContext with connection string
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("eCommerceConnection")));

// Thêm SharedServices
builder.Services.AddSharedServices<UserDbContext>(builder.Configuration, "UserServiceLogs");

// Đăng ký JwtTokenGenerator
builder.Services.AddSingleton<JwtTokenGenerator>();


var connectionString = builder.Configuration.GetConnectionString("eCommerceConnection");
Log.Information($"Connection string: {connectionString}");


    

builder.Services.AddControllers();


var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalException>();
app.UseSharedPolicies();
app.MapControllers();
app.Run();
