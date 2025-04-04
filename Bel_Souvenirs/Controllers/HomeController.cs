using System.Diagnostics;
using System.Security.Claims;
using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
          
        }

        public async Task<IActionResult> Index([FromServices] CartService cartService)
        {
            var products = _appDbContext.Products.ToList();
            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;

            ViewBag.CartItemCount = await cartService.GetCartItemsCountAsync();
            
            
            var productIds = userId != null ? 
                await cartService.GetCartItemsIdsAsync(userId) : new List<int>();

            var productsModel = products.Select(p => new ProductViewModel
            {
                product = p,
                isInCart = productIds.Contains(p.Id)
            }).ToList();

            return View(productsModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
