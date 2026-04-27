using AmazonClone.Api.Data;
using AmazonClone.Api.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AmazonClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // DTO for returning products without cycles
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public string? ImageUrl { get; set; }
            public string? CategoryName { get; set; }
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category != null ? p.Category.Name : null
                })
                .ToListAsync();

            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category != null ? product.Category.Name : null
            });
        }

        // PUT: api/products/{id} -> update an entire product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest();

            _context.Entry(updatedProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/products/{id}/category/{categoryId} -> update only category
        [HttpPatch("{id}/category/{categoryId}")]
        public async Task<IActionResult> UpdateProductCategory(int id, int categoryId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.CategoryId = categoryId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
