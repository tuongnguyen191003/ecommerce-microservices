using Microsoft.EntityFrameworkCore;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; } // Thay thế Manufacturer bằng Brand
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantDiscount> ProductVariantDiscounts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships and table constraints

            // Category to Product - 1:n relationship (One category can have many products)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);  // Giảm nguy cơ cascade vòng lặp

            // Brand to Product - 1:n relationship (One brand can have many products)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product to ProductVariant - 1:n relationship (One product can have many variants)
            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVariants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductVariant to ProductVariantDiscount - 1:n relationship (One product variant can have many discounts)
            modelBuilder.Entity<ProductVariantDiscount>()
                .HasOne(pvd => pvd.ProductVariant)
                .WithMany(pv => pv.ProductVariantDiscounts)
                .HasForeignKey(pvd => pvd.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductDiscount to Product - n:1 relationship (One product can have many discounts)
            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Product)
                .WithMany(p => p.ProductDiscounts)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductDiscount to Discount - n:1 relationship (One discount can apply to many products)
            modelBuilder.Entity<ProductDiscount>()
                .HasOne(pd => pd.Discount)
                .WithMany(d => d.ProductDiscounts)
                .HasForeignKey(pd => pd.DiscountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
