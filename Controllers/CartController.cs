using AmazonClone.Api.Data;
using AmazonClone.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AmazonClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(userId);
        }
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return Ok(new { Items = new List<CartItem>() });

            return Ok(cart);
        }
        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
                item.Quantity++;
            else
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1
                });

            await _context.SaveChangesAsync();

            return Ok(cart);
        }
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                return NotFound("Item not found in cart");

            if (quantity <= 0)
            {
                cart.Items.Remove(item); // optional: remove if 0
            }
            else
            {
                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();

            return Ok(cart);
        }
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                return NotFound("Item not found");

            cart.Items.Remove(item);

            await _context.SaveChangesAsync();

            return Ok(cart);
        }
    }
}