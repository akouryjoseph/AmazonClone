using AmazonClone.Api.Data;
using AmazonClone.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazonClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor: inject the DbContext so we can interact with the database
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        // Returns a list of all categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // POST: api/categories
        // Adds a new category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category); // Returns the category we just added
        }

        // PUT: api/categories/{id}
        // Updates a category (for example, change the name)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category updatedCategory)
        {
            if (id != updatedCategory.Id)
                return BadRequest(); // The id in URL must match the id in the body

            _context.Entry(updatedCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent(); // 204 No Content is standard for successful PUT
        }

        // DELETE: api/categories/{id}
        // Deletes a category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(); // Return 404 if the category doesn't exist

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
