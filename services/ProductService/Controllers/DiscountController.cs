using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

namespace DiscountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public DiscountController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: /api/discounts
        [HttpGet]
        public async Task<IActionResult> GetDiscounts()
        {
            var discounts = await _context.Discounts
                .Select(d => new DiscountDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Description = d.Description,
                    DiscountType = d.DiscountType,
                    DiscountValue = d.DiscountValue,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    IsActive = d.IsActive,
                    MinPurchaseAmount = d.MinPurchaseAmount,
                    MaxDiscountAmount = d.MaxDiscountAmount
                }).ToListAsync();

            return Ok(discounts);
        }

        // GET: /api/discounts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var discount = await _context.Discounts
                .Where(d => d.Id == id)
                .Select(d => new DiscountDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Description = d.Description,
                    DiscountType = d.DiscountType,
                    DiscountValue = d.DiscountValue,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    IsActive = d.IsActive,
                    MinPurchaseAmount = d.MinPurchaseAmount,
                    MaxDiscountAmount = d.MaxDiscountAmount
                }).FirstOrDefaultAsync();

            if (discount == null)
            {
                return NotFound(new { message = "Discount not found." });
            }

            return Ok(discount);
        }

        // POST: /api/admin/discounts
        [HttpPost("/api/admin/discounts")]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var discount = new Discount
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MinPurchaseAmount = dto.MinPurchaseAmount,
                MaxDiscountAmount = dto.MaxDiscountAmount,
                IsActive = true // Mặc định là hoạt động
            };

            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDiscountById), new { id = discount.Id }, discount);
        }

        // PUT: /api/admin/discounts/{id}
        [HttpPut("/api/admin/discounts/{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountDto dto)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound(new { message = "Discount not found" });
            }

            discount.Code = dto.Code;
            discount.Name = dto.Name;
            discount.Description = dto.Description;
            discount.DiscountType = dto.DiscountType;
            discount.DiscountValue = dto.DiscountValue;
            discount.StartDate = dto.StartDate;
            discount.EndDate = dto.EndDate;
            discount.MinPurchaseAmount = dto.MinPurchaseAmount;
            discount.MaxDiscountAmount = dto.MaxDiscountAmount;

            await _context.SaveChangesAsync();

            return Ok(discount);
        }

        // DELETE: /api/admin/discounts/{id}
        [HttpDelete("/api/admin/discounts/{id}")]
        
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound(new { message = "Discount not found" });
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
