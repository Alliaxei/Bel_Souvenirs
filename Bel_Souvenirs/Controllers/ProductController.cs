using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
