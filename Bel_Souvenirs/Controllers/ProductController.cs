using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class ProductController(AppDbContext context, CartService cartService) : BaseController(cartService)
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isInCart = userId != null && await _cartService.IsProductInCartAsync(id, userId);


            return View(new ProductViewModel 
            {
                Product = product,
                IsInCart = isInCart
            });
         }

        public IActionResult Index()
        {
            return View();
        }
    }
}
