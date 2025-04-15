using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class ContactsController(CartService cartService) : BaseController(cartService)
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
