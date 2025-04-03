using Bel_Souvenirs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _appDbContext;
        
        public CartController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var cart = _appDbContext.Carts
                .Include(c => c.Items) // Этот код привязывает товары к корзине
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);
            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart([FromForm] int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _appDbContext.Carts
                .Include(c => c.Items) //Нужно, чтобы доп данные загружались при нахождении корзины
                .ThenInclude(i => i.Product) // По умолчанию используется ленивая загрузка
                .FirstOrDefault(c => c.UserId == userId);

            var product  = await _appDbContext.Products.FindAsync(productId);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    Product = product,
                });
            }

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
