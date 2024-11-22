using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/[controller]")]
    public class ProductReviewController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductReviewController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: /api/products/{productId}/reviews
        [HttpGet]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            // Retrieve all reviews for a specific product
            var reviews = await _context.ProductReviews
                .AsNoTracking()
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            return Ok(reviews);
        }

        // POST: /api/products/{productId}/reviews
        [HttpPost]
        public async Task<IActionResult> AddProductReview(int productId, [FromBody] ProductReview review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assign productId to the review
            review.ProductId = productId;
            review.CreatedAt = DateTime.UtcNow;

            // Add the review to the database
            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductReviews), new { productId = productId }, review);
        }
    }
} 
