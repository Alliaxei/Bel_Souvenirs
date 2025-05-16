using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class ThemeController : Controller
    {
        [HttpPost]
        public IActionResult Toggle()
        {
            var currentTheme = Request.Cookies["Theme"];

            var newTheme = currentTheme == "dark" ? "light" : "dark";

            Response.Cookies.Append("Theme", newTheme, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                IsEssential = true
            });

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
