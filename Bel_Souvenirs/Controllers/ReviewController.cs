using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Bel_Souvenirs.Controllers
{
    public class ReviewController(IReviewService reviewService) : Controller
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage)
                     .ToList();

                TempData["ReviewErrors"] = JsonSerializer.Serialize(errors);
                return RedirectToAction("Details", "Product", new { id = model.ProductId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? throw new Exception ("Ошибка при идентификации пользователя");

            await _reviewService.AddReviewAsync(model.ProductId, userId, model.Text, model.Rating);


            return RedirectToAction("Details", "Product", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await _reviewService.DeleteReviewAsync(reviewId);

            if (!result)
            {
                TempData["ReviewErrors"] = JsonSerializer.Serialize(new List<string>
                    {
                        "Отзыв не найден или недоступен для удаления."
                    });
            }
            else
            {
                TempData["ReviewSuccess"] = JsonSerializer.Serialize(new List<string>
                    {
                        "Отзыв успешно удалён."
                    });
            }

            return Redirect(Request.Headers.Referer.ToString());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
