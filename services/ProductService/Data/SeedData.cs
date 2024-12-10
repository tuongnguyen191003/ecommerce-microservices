using Microsoft.EntityFrameworkCore;

namespace ProductService.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ProductDbContext>();

            // Kiểm tra xem có dữ liệu trong bảng Brands chưa
            if (await context.Brands.AnyAsync()) return;

            // Thêm các brand
            var brands = new List<Brand>
            {
                new Brand { Name = "Apple", Slug = "apple", Description = "Apple Inc.", Picture = "https://example.com/apple-logo.png" },
                new Brand { Name = "Samsung", Slug = "samsung", Description = "Samsung Electronics", Picture = "https://example.com/samsung-logo.png" },
                new Brand { Name = "Sony", Slug = "sony", Description = "Sony Corporation", Picture = "https://example.com/sony-logo.png" },
                new Brand { Name = "Microsoft", Slug = "microsoft", Description = "Microsoft Corporation", Picture = "https://example.com/microsoft-logo.png" },
                new Brand { Name = "LG", Slug = "lg", Description = "LG Electronics", Picture = "https://example.com/lg-logo.png" }
            };

            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();

            // Kiểm tra xem có dữ liệu trong bảng Categories chưa
            if (await context.Categories.AnyAsync()) return;

            // Tạo các danh mục cha
            var categories = new List<Category>
            {
                new Category 
                { 
                    Name = "Electronics", 
                    Slug = "electronics", 
                    Description = "Electronic devices", 
                    DisplayOrder = 1,  
                    Published = true
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Lấy lại Id của danh mục cha (Electronics)
            var electronicsCategoryId = categories.FirstOrDefault(c => c.Slug == "electronics")?.Id;

            if (electronicsCategoryId.HasValue)
            {
                // Tạo các danh mục con của Electronics
                var subcategories = new List<Category>
                {
                    new Category 
                    { 
                        Name = "Mobiles", 
                        Slug = "mobiles", 
                        ParentCategoryId = electronicsCategoryId.Value, 
                        Description = "Mobile phones and accessories", 
                        DisplayOrder = 2,  
                        Published = true 
                    },
                    new Category 
                    { 
                        Name = "Laptops", 
                        Slug = "laptops", 
                        ParentCategoryId = electronicsCategoryId.Value, 
                        Description = "Laptops and accessories", 
                        DisplayOrder = 3,  
                        Published = true
                    },
                    new Category 
                    { 
                        Name = "Headphones", 
                        Slug = "headphones", 
                        ParentCategoryId = electronicsCategoryId.Value, 
                        Description = "Audio accessories", 
                        DisplayOrder = 4,  
                        Published = true
                    },
                    new Category 
                    { 
                        Name = "Smart Watches", 
                        Slug = "smart-watches", 
                        ParentCategoryId = electronicsCategoryId.Value, 
                        Description = "Wearable technology", 
                        DisplayOrder = 5,  
                        Published = true
                    }
                };

                await context.Categories.AddRangeAsync(subcategories);
                await context.SaveChangesAsync();
            }

            // Kiểm tra xem có dữ liệu trong bảng Products chưa
            if (!await context.Products.AnyAsync())
            {
                // Thêm sản phẩm mẫu
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "iPhone 13",
                        ShortDescription = "Apple iPhone 13",
                        Slug = "iphone-13",
                        LongDescription = "The Apple iPhone 13 with A15 Bionic chip and 5G.",
                        CategoryId = 2, // Mobiles
                        BrandId = 1, // Apple
                        Price = 799,
                        OldPrice = 999,
                        CostPrice = 600,
                        IsPublished = true,
                        SeoUrl = "iphone-13",
                        Picture = "https://example.com/iphone13.png"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S21",
                        Slug = "galaxy-s21",
                        ShortDescription = "Samsung Galaxy S21",
                        LongDescription = "Samsung Galaxy S21 with Snapdragon 888 processor.",
                        CategoryId = 2, // Mobiles
                        BrandId = 2, // Samsung
                        Price = 799,
                        OldPrice = 999,
                        CostPrice = 600,
                        IsPublished = true,
                        SeoUrl = "galaxy-s21",
                        Picture = "https://example.com/galaxy-s21.png"
                    },
                    new Product
                    {
                        Name = "Sony Xperia 5 II",
                        Slug = "xperia-5-ii",
                        ShortDescription = "Sony Xperia 5 II",
                        LongDescription = "Sony Xperia 5 II, professional camera and gaming phone.",
                        CategoryId = 2, // Mobiles
                        BrandId = 3, // Sony
                        Price = 950,
                        OldPrice = 1200,
                        CostPrice = 800,
                        IsPublished = true,
                        SeoUrl = "xperia-5-ii",
                        Picture = "https://example.com/xperia-5-ii.png"
                    },
                    new Product
                    {
                        Name = "MacBook Air",
                        Slug = "macbook-air",
                        ShortDescription = "Apple MacBook Air M1",
                        LongDescription = "MacBook Air with M1 chip, 13-inch display.",
                        CategoryId = 3, // Laptops
                        BrandId = 1, // Apple
                        Price = 999,
                        OldPrice = 1199,
                        CostPrice = 850,
                        IsPublished = true,
                        SeoUrl = "macbook-air",
                        Picture = "https://example.com/macbook-air.png"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Book Pro",
                        Slug = "galaxy-book-pro",
                        ShortDescription = "Samsung Galaxy Book Pro",
                        LongDescription = "Samsung Galaxy Book Pro with Intel Core i7 processor.",
                        CategoryId = 3, // Laptops
                        BrandId = 2, // Samsung
                        Price = 1099,
                        OldPrice = 1300,
                        CostPrice = 950,
                        IsPublished = true,
                        SeoUrl = "galaxy-book-pro",
                        Picture = "https://example.com/galaxy-book-pro.png"
                    },
                    new Product
                    {
                        Name = "Sony WH-1000XM4",
                        Slug = "wh-1000xm4",
                        ShortDescription = "Sony WH-1000XM4 Headphones",
                        LongDescription = "Noise-cancelling over-ear headphones from Sony.",
                        CategoryId = 4, // Headphones
                        BrandId = 3, // Sony
                        Price = 350,
                        OldPrice = 400,
                        CostPrice = 300,
                        IsPublished = true,
                        SeoUrl = "wh-1000xm4",
                        Picture = "https://example.com/wh-1000xm4.png"
                    },
                    new Product
                    {
                        Name = "Apple Watch Series 7",
                        Slug = "apple-watch-series-7",
                        ShortDescription = "Apple Watch Series 7",
                        LongDescription = "Apple Watch Series 7 with larger display and fast charging.",
                        CategoryId = 5, // Smart Watches
                        BrandId = 1, // Apple
                        Price = 399,
                        OldPrice = 450,
                        CostPrice = 350,
                        IsPublished = true,
                        SeoUrl = "apple-watch-series-7",
                        Picture = "https://example.com/apple-watch-series-7.png"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Watch 4",
                        Slug = "galaxy-watch-4",
                        ShortDescription = "Samsung Galaxy Watch 4",
                        LongDescription = "Samsung Galaxy Watch 4 with Wear OS and body composition measurement.",
                        CategoryId = 5, // Smart Watches
                        BrandId = 2, // Samsung
                        Price = 250,
                        OldPrice = 300,
                        CostPrice = 200,
                        IsPublished = true,
                        SeoUrl = "galaxy-watch-4",
                        Picture = "https://example.com/galaxy-watch-4.png"
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // Kiểm tra xem có dữ liệu trong bảng ProductVariants chưa
            if (!await context.ProductVariants.AnyAsync())
            {
                var variants = new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        ProductId = 1,
                        Color = "Red",
                        Storage = "128GB",
                        Price = 799,
                        OldPrice = 999
                    },
                    new ProductVariant
                    {
                        ProductId = 2,
                        Color = "Black",
                        Storage = "128GB",
                        Price = 799,
                        OldPrice = 999
                    },
                    new ProductVariant
                    {
                        ProductId = 3,
                        Color = "Blue",
                        Storage = "128GB",
                        Price = 950,
                        OldPrice = 1200
                    }
                };

                await context.ProductVariants.AddRangeAsync(variants);
                await context.SaveChangesAsync();
            }
        }
    }
}
