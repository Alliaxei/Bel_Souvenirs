using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bel_Souvenirs.Controllers
{
    public class BaseController(CartService cartService) : Controller
    {

        public readonly CartService _cartService = cartService;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (HttpContext.Items.ContainsKey("FullName"))
            {
                ViewBag.FullName = HttpContext.Items["FullName"]?.ToString();
            }

            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();

            await next();

        }
    }
}
