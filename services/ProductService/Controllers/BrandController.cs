using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public BrandController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: /api/brands
        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Brands
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    IsActive = b.IsActive,
                    Slug = b.Slug
                }).ToListAsync();

            if (brands == null || brands.Count == 0)
            {
                return NotFound(new { message = "No brands found." });
            }

            return Ok(brands);
        }

        // GET: /api/brands/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _context.Brands
                .Where(b => b.Id == id)
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    IsActive = b.IsActive,
                    Slug = b.Slug
                }).FirstOrDefaultAsync();

            if (brand == null)
            {
                return NotFound(new { message = "Brand not found." });
            }

            return Ok(brand);
        }

        // POST: /api/admin/brands
        [HttpPost]
        [Route("/api/admin/brands")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBrand([FromBody] CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var slug = GenerateSlug(dto.Name);

            var brand = new Brand
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Slug = slug.ToLower(),
                Picture = dto.Picture
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, brand);
        }


        // PUT: /api/admin/brands/{id}
        [HttpPut]
        [Route("/api/admin/brands/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandDto dto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound(new { message = "Brand not found" });
            }

            brand.Name = dto.Name;
            brand.Description = dto.Description;
            brand.IsActive = dto.IsActive;
            brand.Picture = dto.Picture; 

            await _context.SaveChangesAsync();

            return Ok(brand);
        }


        // DELETE: /api/admin/brands/{id}
        [HttpDelete]
        [Route("/api/admin/brands/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound(new { message = "Brand not found" });
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Hàm tạo Slug từ tên
        private string GenerateSlug(string name)
        {
            return name.ToLower().Replace(" ", "-");
        }
    }
}
