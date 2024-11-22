using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/admin/brands")]
    public class AdminBrandController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public AdminBrandController(ProductDbContext context)
        {
            _context = context;
        }

        // POST: /api/admin/brands
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddBrand), new { id = brand.Id }, brand);
        }

        // PUT: /api/admin/brands/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] Brand updatedBrand)
        {
            if (id != updatedBrand.Id)
            {
                return BadRequest(new { message = "Brand ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound(new { message = "Brand not found." });
            }

            brand.Name = updatedBrand.Name;
            brand.Slug = updatedBrand.Slug;
            brand.Description = updatedBrand.Description;
            brand.LogoUrl = updatedBrand.LogoUrl;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/admin/brands/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound(new { message = "Brand not found." });
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
