using Microsoft.EntityFrameworkCore;
using ProductService.Data.Entities;

public class ProductDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public ProductDbContext(DbContextOptions<ProductDbContext> options, IConfiguration? configuration = null)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Sử dụng chuỗi kết nối từ cấu hình (fallback khi chạy lệnh Update-Database)
            var connectionString = _configuration?.GetConnectionString("ProductServiceConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ConnectionString is not provided or initialized.");
            }
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    // Định nghĩa DbSet cho các bảng
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategoryMapping> ProductCategoryMappings { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductOption> ProductOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình quan hệ danh mục cha/con
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
