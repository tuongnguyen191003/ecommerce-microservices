using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Data.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly IDistributedCache _cache;


        public ProductController(ProductDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? categoryId, [FromQuery] string? brandId, [FromQuery] string? sortBy, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            string cacheKey = $"productList_{categoryId}_{brandId}_{sortBy}_{page}_{pageSize}";
            string serializedProductList;
            var productList = new List<Product>();

            var redisProductList = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(redisProductList))
            {
                serializedProductList = redisProductList;
                productList = JsonSerializer.Deserialize<List<Product>>(serializedProductList);
            }
            else
            {
                var productsQuery = _context.Products.AsNoTracking().AsQueryable();

                // Lọc sản phẩm theo danh mục
                if (!string.IsNullOrEmpty(categoryId))
                {
                    productsQuery = productsQuery.Where(p => p.ProductCategories.Any(m => m.CategoryId.ToString() == categoryId));
                }

                // Lọc sản phẩm theo thương hiệu
                if (!string.IsNullOrEmpty(brandId))
                {
                    productsQuery = productsQuery.Where(p => p.BrandId.ToString() == brandId);
                }

                // Sắp xếp sản phẩm
                productsQuery = sortBy switch
                {
                    "price" => productsQuery.OrderBy(p => p.DefaultPrice),
                    "price_desc" => productsQuery.OrderByDescending(p => p.DefaultPrice),
                    "name" => productsQuery.OrderBy(p => p.Name),
                    "name_desc" => productsQuery.OrderByDescending(p => p.Name),
                    "createdAt" => productsQuery.OrderBy(p => p.CreatedAt),
                    "createdAt_desc" => productsQuery.OrderByDescending(p => p.CreatedAt),
                    _ => productsQuery.OrderBy(p => p.Id),
                };

                // Phân trang sản phẩm
                var totalItems = await productsQuery.CountAsync();
                productList = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                serializedProductList = JsonSerializer.Serialize(productList);
                await _cache.SetStringAsync(cacheKey, serializedProductList, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return Ok(new PagedResponse<Product>(productList, page, pageSize, productList.Count));
        }

        // GET: /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Variants)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        // GET: /api/products/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Query is required." });
            }

            query = query.ToLower();

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query))
                .ToListAsync();

            return Ok(products);
        }

        // GET: /api/products/featured
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] decimal? maxPrice = 1000)
        {
            var featuredProducts = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && (!maxPrice.HasValue || p.DefaultPrice <= maxPrice.Value))
                .ToListAsync();

            return Ok(featuredProducts);
        }
    }
}