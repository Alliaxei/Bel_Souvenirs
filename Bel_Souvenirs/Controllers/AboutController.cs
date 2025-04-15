﻿using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bel_Souvenirs.Controllers
{
    public class AboutController(CartService cartService) : BaseController(cartService)
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
