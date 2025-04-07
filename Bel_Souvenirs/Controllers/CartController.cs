using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class CartController(AppDbContext appDbContext, CartService cartService) : Controller
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly CartService _cartService = cartService;

        public async Task<IActionResult> Index()
        {

            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _appDbContext.Carts
                .Include(c => c.Items) // Привязка товаров к корзине
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            if (userId == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            var result = await _cartService.AddToCartAsync(productId, userId);

            if (!result)
            {
                return Json(new { success = false, message = "Ошибка при добавлении товара в корзину" });
            }

            var cartCount = await _cartService.GetCartItemsCountAsync();
            return Json(new { success = true, cartCount });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

            if (userId == null) // В теории такого произойти не должно
                return Json(new { success = false, message = "Требуется авторизация" });

            var result = await _cartService.DeleteFromCartAsync(productId, userId);

            if (!result)
            {
                return Json(new { success = false, message = "Ошибка при удалении из корзины" });
            }

            var cartCount = await _cartService.GetCartItemsCountAsync();
            return Json(new { success = true, cartCount });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            var result = await _cartService.RemoveItemAsync(itemId, userId);

            if (!result)
                return Json(new { success = false, message = "Не удалось удалить товар" });

            var cartCount = await _cartService.GetCartItemsCountAsync();

            var cart = await _cartService.GetCartAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == itemId);

            return Json(new
            {
                success = true,
                cartCount,
                cartTotal = cart?.Items.Sum(i => i.Quantity * i.Product.Price).ToString("C"),
                totalItems = cart?.Items.Sum(i => i.Quantity)
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            var result = await _cartService.UpdateQuantityAsync(itemId, quantity, userId);

            if (!result)
                return Json(new { success = false, message = "Не удалось изменить количество товаров" });

            var cart = await _cartService.GetCartAsync(userId);
            var item = cart?.Items.FirstOrDefault(i => i.Id == itemId);

            return Json(new
            {
                success = true,
                itemTotal = (item!.Quantity * item.Product.Price).ToString("C"),
                cartTotal = cart?.Items.Sum(i => i.Quantity * i.Product.Price).ToString("C"),
                totalItems = cart?.Items.Sum(i => i.Quantity)
            });

        }
    }
}
