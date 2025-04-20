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

            var productsModel = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var averageRating = await _productService.GetAverageRatingAsync(product.Id);

                productsModel.Add(new ProductViewModel
                {
                    Product = product,
                    IsInCart = productIds.Contains(product.Id),
                    AverageRating = averageRating,
                });
            }

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

        [HttpGet]
        public async Task<IActionResult> LoadProducts(int offset)
        {

            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var productIdsInCart = userId != null ? await _cartService.GetCartItemsIdsAsync(userId) : [];

            var products = await _productService.GetAllProducts(offset, 9);

            var productsModel = new List<ProductViewModel>();

            foreach(var product in products)
            {
                var averageRating = await _productService.GetAverageRatingAsync(product.Id);

                productsModel.Add(new ProductViewModel
                {
                    Product = product,
                    AverageRating = averageRating,
                    IsInCart = productIdsInCart.Contains(product.Id)

                });
            }

            return PartialView("_ProductCardsPartial", productsModel);
        }
    }
}
