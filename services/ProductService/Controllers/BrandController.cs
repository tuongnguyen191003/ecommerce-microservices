using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

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
            // Retrieve all brands
            var brands = await _context.Brands.AsNoTracking().ToListAsync();
            return Ok(brands);
        }

        // GET: /api/brands/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            // Retrieve a specific brand by ID
            var brand = await _context.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
            {
                return NotFound(new { message = "Brand not found." });
            }

            return Ok(brand);
        }
    }
} 
