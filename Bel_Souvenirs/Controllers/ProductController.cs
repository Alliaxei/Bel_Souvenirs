using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _context;
        private readonly CartService _cartService;

        public ProductController(AppDbContext context, CartService cartService, ILogger<ProductController> logger)
        {
            _context = context;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isInCart = userId != null && await _cartService.IsProductInCartAsync(id, userId);

            return View(new ProductViewModel 
            {
                product = product,
                isInCart = isInCart
            });
         }




        public IActionResult Index()
        {
            return View();
        }
    }
}
