using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using SharedLibrary.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Đọc connection string từ cấu hình
//string connectionString = builder.Configuration.GetConnectionString("ProductServiceConnection");

// Thêm DbContext cho ProductService
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductServiceConnection")));

// Tích hợp SharedLibrary để sử dụng các dịch vụ chung như Serilog
builder.Services.AddSharedServices<ProductDbContext>(builder.Configuration, "ProductServiceLogs");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
    options.InstanceName = "ProductServiceCache_";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Sử dụng Serilog để ghi log
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // JWT Authentication từ SharedLibrary
app.UseAuthorization();

app.MapControllers();

app.Run();
