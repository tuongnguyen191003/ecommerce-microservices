using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

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
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                return NotFound(new { message = "No categories found." });
            }

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Published = c.Published,
                DisplayOrder = c.DisplayOrder
            }).ToList();

            return Ok(result);
        }

        // GET: /api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            var result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Published = category.Published,
                DisplayOrder = category.DisplayOrder
            };

            return Ok(result);
        }

        // POST: /api/admin/categories
        [HttpPost]
        [Route("/api/admin/categories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Published = dto.Published,
                DisplayOrder = dto.DisplayOrder,
                Slug = GenerateSlug(dto.Name)  // Tạo slug từ tên
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        private string GenerateSlug(string name)
        {
            // Logic tạo Slug từ Name, có thể sử dụng một thư viện để chuẩn hóa, ví dụ như:
            return name.ToLower().Replace(" ", "-");  // Ví dụ đơn giản
        }


        // PUT: /api/admin/categories/{id}
        [HttpPut]
        [Route("/api/admin/categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Published = dto.Published;
            category.DisplayOrder = dto.DisplayOrder;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // DELETE: /api/admin/categories/{id}
        [HttpDelete]
        [Route("/api/admin/categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
