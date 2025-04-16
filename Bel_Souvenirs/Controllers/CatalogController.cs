using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class CatalogController(CartService cartService, IProductService productService) : BaseController(cartService)
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(string searchString, string category, string sortOrder)
        {

            var filteredProducts = await _productService.GetFilteredProductsAsync(searchString, category, sortOrder);

            var userId = User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var productIds = userId != null ?
                await _cartService.GetCartItemsIdsAsync(userId) : [];


            var productsModel = filteredProducts.Select(p => new ProductViewModel
            {
                Product = p,
                IsInCart = productIds.Contains(p.Id)
            }).ToList();

            ViewBag.Categories = await _productService.GetCategoryNamesAsync();
            ViewBag.CurrentCategory = category;
            ViewBag.NameSort = sortOrder == "name_desc" ? "name" : "name_desc";
            ViewBag.PriceSort = sortOrder == "price_desc" ? "price" : "price_desc";
            ViewBag.PopularSort = sortOrder == "popular" ? null : "popular";
            return View(productsModel);
        }
    }
}
