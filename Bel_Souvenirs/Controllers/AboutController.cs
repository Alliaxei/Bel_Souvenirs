using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class AboutController : Controller
    {
        public async Task<IActionResult> Index([FromServices] CartService cartService)
        {
            ViewBag.CartItemCount = await cartService.GetCartItemsCountAsync();


            return View();
        }
    }
}
