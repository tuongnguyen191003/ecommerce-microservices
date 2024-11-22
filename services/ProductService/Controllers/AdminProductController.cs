using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly IDistributedCache _cache;

        public AdminProductController(ProductDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        // POST: /api/admin/products
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Xóa cache khi thêm sản phẩm mới
            await ClearProductListCache();

            return CreatedAtAction(nameof(AddProduct), new { id = product.Id }, product);
        }

        private async Task ClearProductListCache()
        {
            var cacheKeys = new List<string> { "productList_" }; // Replace with appropriate pattern matching logic if needed
            foreach (var key in cacheKeys)
            {
                await _cache.RemoveAsync(key);
            }
        }

        // PUT: /api/admin/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest(new { message = "Product ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            product.Name = updatedProduct.Name;
            product.Slug = updatedProduct.Slug;
            product.Description = updatedProduct.Description;
            product.BrandId = updatedProduct.BrandId;
            product.DefaultPrice = updatedProduct.DefaultPrice;
            product.IsActive = updatedProduct.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/admin/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: /api/admin/products/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateProductStatus(int id, [FromBody] bool isActive)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            product.IsActive = isActive;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: /api/admin/products/{id}/inventory
        [HttpPut("{id}/inventory")]
        public async Task<IActionResult> UpdateProductInventory(int id, [FromBody] int stockQuantity)
        {
            var product = await _context.Products.Include(p => p.Variants).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            foreach (var variant in product.Variants)
            {
                variant.StockQuantity = stockQuantity;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: /api/admin/products/{id}/variants
        [HttpPost("{id}/variants")]
        public async Task<IActionResult> AddProductVariant(int id, [FromBody] ProductVariant variant)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            variant.ProductId = id;
            variant.CreatedAt = DateTime.UtcNow;

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddProduct), new { id = variant.Id }, variant);
        }

        // PUT: /api/admin/products/{productId}/variants/{variantId}
        [HttpPut("{productId}/variants/{variantId}")]
        public async Task<IActionResult> UpdateProductVariant(int productId, int variantId, [FromBody] ProductVariant updatedVariant)
        {
            if (variantId != updatedVariant.Id)
            {
                return BadRequest(new { message = "Variant ID mismatch." });
            }

            var variant = await _context.ProductVariants.FindAsync(variantId);
            if (variant == null || variant.ProductId != productId)
            {
                return NotFound(new { message = "Variant not found." });
            }

            variant.OptionName = updatedVariant.OptionName;
            variant.Price = updatedVariant.Price;
            variant.SKU = updatedVariant.SKU;
            variant.StockQuantity = updatedVariant.StockQuantity;
            variant.UpdatedAt = DateTime.UtcNow;

            _context.ProductVariants.Update(variant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/admin/products/{productId}/variants/{variantId}
        [HttpDelete("{productId}/variants/{variantId}")]
        public async Task<IActionResult> DeleteProductVariant(int productId, int variantId)
        {
            var variant = await _context.ProductVariants.FindAsync(variantId);
            if (variant == null || variant.ProductId != productId)
            {
                return NotFound(new { message = "Variant not found." });
            }

            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
