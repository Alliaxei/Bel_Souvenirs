using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class CatalogController(CartService cartService, IProductService productService) : BaseController(cartService)
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(string searchString, string category, string sortOrder)
        {

            ViewBag.SearchString = searchString;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSort = sortOrder;


            var filteredProducts = await _productService.GetFilteredProductsAsync(searchString, category, sortOrder);

            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var productIds = userId != null ? await _cartService.GetCartItemsIdsAsync(userId) : [];


            var productsModel = new List<ProductViewModel>();
            foreach (var product in filteredProducts)
            {
                var averageRating = await _productService.GetAverageRatingAsync(product.Id);

                productsModel.Add(new ProductViewModel
                {
                    Product = product,
                    IsInCart = productIds.Contains(product.Id),
                    AverageRating = averageRating,
                });
            }

            ViewBag.Categories = await _productService.GetCategoryNamesAsync();
            ViewBag.CurrentCategory = category;
            ViewBag.NameSort = sortOrder == "name_desc" ? "name" : "name_desc";
            ViewBag.PriceSort = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.PopularSort = sortOrder == "popular" ? null : "popular";
            return View(productsModel);
        }
    }
}
