using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/admin/categories")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public AdminCategoryController(ProductDbContext context)
        {
            _context = context;
        }

        // POST: /api/admin/categories
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddCategory), new { id = category.Id }, category);
        }

        // PUT: /api/admin/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            if (id != updatedCategory.Id)
            {
                return BadRequest(new { message = "Category ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            category.Name = updatedCategory.Name;
            category.Slug = updatedCategory.Slug;
            category.ParentCategoryId = updatedCategory.ParentCategoryId;
            category.Description = updatedCategory.Description;
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/admin/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: /api/admin/categories/{categoryId}/subcategories
        [HttpPost("{categoryId}/subcategories")]
        public async Task<IActionResult> AddSubCategory(int categoryId, [FromBody] Category subCategory)
        {
            var parentCategory = await _context.Categories.FindAsync(categoryId);
            if (parentCategory == null)
            {
                return NotFound(new { message = "Parent category not found." });
            }

            subCategory.ParentCategoryId = categoryId;
            subCategory.CreatedAt = DateTime.UtcNow;
            subCategory.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Add(subCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddSubCategory), new { id = subCategory.Id }, subCategory);
        }

        // PUT: /api/admin/subcategories/{subcategoryId}
        [HttpPut("subcategories/{subcategoryId}")]
        public async Task<IActionResult> UpdateSubCategory(int subcategoryId, [FromBody] Category updatedSubCategory)
        {
            if (subcategoryId != updatedSubCategory.Id)
            {
                return BadRequest(new { message = "Subcategory ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subCategory = await _context.Categories.FindAsync(subcategoryId);
            if (subCategory == null || subCategory.ParentCategoryId == null)
            {
                return NotFound(new { message = "Subcategory not found." });
            }

            subCategory.Name = updatedSubCategory.Name;
            subCategory.Slug = updatedSubCategory.Slug;
            subCategory.Description = updatedSubCategory.Description;
            subCategory.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(subCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/admin/subcategories/{subcategoryId}
        [HttpDelete("subcategories/{subcategoryId}")]
        public async Task<IActionResult> DeleteSubCategory(int subcategoryId)
        {
            var subCategory = await _context.Categories.FindAsync(subcategoryId);
            if (subCategory == null || subCategory.ParentCategoryId == null)
            {
                return NotFound(new { message = "Subcategory not found." });
            }

            _context.Categories.Remove(subCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
