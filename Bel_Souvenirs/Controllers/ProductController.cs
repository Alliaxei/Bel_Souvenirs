using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class ProductController(AppDbContext context, CartService cartService, IReviewService reviewService, IProductService productService) : BaseController(cartService)
    {
        private readonly AppDbContext _context = context;
        private readonly IReviewService _reviewService = reviewService;
        private readonly IProductService _productService = productService;
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isInCart = userId != null && await _cartService.IsProductInCartAsync(id, userId);

            var Reviews = await _reviewService.GetAllReviewsAsync(id);
            var averageRating = await _productService.GetAverageRatingAsync(id);
            return View(new ProductViewModel
            {
                Product = product,
                IsInCart = isInCart,
                Reviews = Reviews,
                AverageRating = averageRating
            });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
