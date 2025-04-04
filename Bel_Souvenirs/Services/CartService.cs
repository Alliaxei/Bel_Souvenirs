using Bel_Souvenirs.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Services
{
    public class CartService
    {
        private readonly AppDbContext _appDbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public CartService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetCartItemsCountAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return 0;
            }

            return await _appDbContext.CartItems
                .Where(ci => ci.Cart.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<bool> AddToCartAsync(int productId, string userId)
        {
            var cart = await _appDbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = 1
            });

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFromCartAsync(int productId, string userId)
        {
            if (userId == null)
                return false;

            var cart = await _appDbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return true;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return false;

            cart.Items.Remove(item);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
        public async Task<List<int>> GetCartItemsIdsAsync(string userId)
        {
            return await _appDbContext.Carts
                .Where(c => c.UserId == userId)
                .SelectMany(c => c.Items.Select(i => i.ProductId))
                .ToListAsync();
        }


    }
}
