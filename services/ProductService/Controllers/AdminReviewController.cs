using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/admin/products/{productId}/reviews")]
    public class AdminReviewController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public AdminReviewController(ProductDbContext context)
        {
            _context = context;
        }

        // DELETE: /api/admin/products/{productId}/reviews/{reviewId}
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int productId, int reviewId)
        {
            var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.ProductId == productId);
            if (review == null)
            {
                return NotFound(new { message = "Review not found." });
            }

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: /api/admin/products/{productId}/reviews/{reviewId}
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int productId, int reviewId, [FromBody] ProductReview updatedReview)
        {
            if (reviewId != updatedReview.Id || productId != updatedReview.ProductId)
            {
                return BadRequest(new { message = "Review ID or Product ID mismatch." });
            }

            var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.ProductId == productId);
            if (review == null)
            {
                return NotFound(new { message = "Review not found." });
            }

            review.Rating = updatedReview.Rating;
            review.Review = updatedReview.Review;
            review.UserName = updatedReview.UserName;
            review.CreatedAt = updatedReview.CreatedAt;

            _context.ProductReviews.Update(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
