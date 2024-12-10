using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    LongDescription = p.LongDescription,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    IsPublished = p.IsPublished,
                    Tags = p.Tags,
                    IsShowHomePage = p.IsShowHomePage,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    CostPrice = p.CostPrice,
                    IsBuyButton = p.IsBuyButton,
                    IsWishList = p.IsWishList,
                    MetaTitle = p.MetaTitle,
                    MetaDescription = p.MetaDescription,
                    MetaKeywords = p.MetaKeywords,
                    SeoUrl = p.SeoUrl,
                    Picture = p.Picture,
                    Video = p.Video,
                    Slug = p.Slug
                }).ToListAsync();

            return Ok(products);
        }

        // GET: /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    LongDescription = p.LongDescription,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    IsPublished = p.IsPublished,
                    Tags = p.Tags,
                    IsShowHomePage = p.IsShowHomePage,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    CostPrice = p.CostPrice,
                    IsBuyButton = p.IsBuyButton,
                    IsWishList = p.IsWishList,
                    MetaTitle = p.MetaTitle,
                    MetaDescription = p.MetaDescription,
                    MetaKeywords = p.MetaKeywords,
                    SeoUrl = p.SeoUrl,
                    Picture = p.Picture,
                    Video = p.Video,
                    Slug = p.Slug
                }).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        // POST: /api/admin/products
        [HttpPost]
        [Route("/api/admin/products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Tạo slug tự động cho sản phẩm
            var slug = GenerateSlug(dto.Name);

            var product = new Product
            {
                Name = dto.Name,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                IsPublished = dto.IsPublished,
                Tags = dto.Tags,
                IsShowHomePage = dto.IsShowHomePage,
                Price = dto.Price,
                OldPrice = dto.OldPrice,
                CostPrice = dto.CostPrice,
                IsBuyButton = dto.IsBuyButton,
                IsWishList = dto.IsWishList,
                MetaTitle = dto.MetaTitle,
                MetaDescription = dto.MetaDescription,
                MetaKeywords = dto.MetaKeywords,
                SeoUrl = dto.SeoUrl,
                Picture = dto.Picture,
                Video = dto.Video,
                Slug = slug.ToLower()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT: /api/admin/products/{id}
        [HttpPut]
        [Route("/api/admin/products/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            product.Name = dto.Name;
            product.ShortDescription = dto.ShortDescription;
            product.LongDescription = dto.LongDescription;
            product.CategoryId = dto.CategoryId;
            product.BrandId = dto.BrandId;
            product.IsPublished = dto.IsPublished;
            product.Tags = dto.Tags;
            product.IsShowHomePage = dto.IsShowHomePage;
            product.Price = dto.Price;
            product.OldPrice = dto.OldPrice;
            product.CostPrice = dto.CostPrice;
            product.IsBuyButton = dto.IsBuyButton;
            product.IsWishList = dto.IsWishList;
            product.MetaTitle = dto.MetaTitle;
            product.MetaDescription = dto.MetaDescription;
            product.MetaKeywords = dto.MetaKeywords;
            product.SeoUrl = dto.SeoUrl;
            product.Picture = dto.Picture;
            product.Video = dto.Video;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // DELETE: /api/admin/products/{id}
        [HttpDelete]
        [Route("/api/admin/products/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Hàm tạo slug từ tên sản phẩm
        private string GenerateSlug(string name)
        {
            return name.ToLower().Replace(" ", "-");
        }
    }
}
