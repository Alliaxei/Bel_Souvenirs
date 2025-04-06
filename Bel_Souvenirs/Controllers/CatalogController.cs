using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class CatalogController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly CartService _cartService;
        public CatalogController(AppDbContext dbContext, CartService cartService)
        {
            _dbContext = dbContext;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index(
            string searchString, 
            string category, 
            string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSort"] = sortOrder == "Price" ? "price_desc" : "Price";


            var products = from p in _dbContext.Products select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString)
                        || p.Description.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();

            var filteredProducts = await products.ToListAsync();

            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var productIds = userId != null ?
                await _cartService.GetCartItemsIdsAsync(userId) : new List<int>();


            var productsModel = filteredProducts.Select(p => new ProductViewModel
            {
                product = p,
                isInCart = productIds.Contains(p.Id)
            }).ToList();

            var categories = await _dbContext.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();

            return View(productsModel);
        }
    }
}
