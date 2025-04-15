using System.Diagnostics;
using System.Security.Claims;
using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class HomeController(CartService cartService, IProductService productService) : BaseController(cartService)
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index([FromServices] CartService cartService)
        {
            var products = await _productService.GetAllProducts();

            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            
            var productIds = userId != null 
                ? await cartService.GetCartItemsIdsAsync(userId) 
                : [];

            var productsModel =  products.Select(p => new ProductViewModel
            {
                Product = p,
                IsInCart = productIds.Contains(p.Id)
            }).ToList();

            var topPopular = productsModel.
                OrderByDescending(p => p.Product.AmountOfOrders)
                .Take(4)
                .ToList();

            var model = new HomeViewModel
            {
                AllProducts = productsModel,
                TopPopularProducts = topPopular
            };

            return View(model);
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
