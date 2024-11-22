using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public CategoryController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: /api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null)
        {
            var categoriesQuery = _context.Categories.AsNoTracking().AsQueryable();

            // Apply sorting
            categoriesQuery = sortBy switch
            {
                "name" => categoriesQuery.OrderBy(c => c.Name),
                "name_desc" => categoriesQuery.OrderByDescending(c => c.Name),
                "createdAt" => categoriesQuery.OrderBy(c => c.CreatedAt),
                "createdAt_desc" => categoriesQuery.OrderByDescending(c => c.CreatedAt),
                _ => categoriesQuery.OrderBy(c => c.Id),
            };

            // Apply pagination
            var totalItems = await categoriesQuery.CountAsync();
            var categories = await categoriesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new { Page = page, PageSize = pageSize, TotalItems = totalItems, Items = categories });
        }

        // GET: /api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            // Retrieve a specific category by ID
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            return Ok(category);
        }

        // GET: /api/categories/{categoryId}/subcategories
        [HttpGet("{categoryId}/subcategories")]
        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            return Ok(category.SubCategories);
        }
    }
} 
