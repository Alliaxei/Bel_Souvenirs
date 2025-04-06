using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _appDbContext;
        private readonly CartService _cartService;
        private readonly EmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext appDbContext,
            CartService cartService,
            EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
            _cartService = cartService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailConfirmationSent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailConfirmed()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = "",
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var cart = new Cart
                    {
                        UserId = user.Id,

                        Items = new List<CartItem>()
                    };

                    _appDbContext.Carts.Add(cart);
                    await _appDbContext.SaveChangesAsync();

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationUrl = Url.Action("ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = WebUtility.UrlEncode(token) },
                        Request.Scheme);

                    var subject = "Подтверждение Email";
                    var body =  $"<p>Здравствуйте, {user.UserName}!</p>" +
                                       $"<p>Пожалуйста, подтвердите вашу почту, перейдя по ссылке:</p>" +
                                       $"<p><a href='{confirmationUrl}'>Подтвердить email</a></p>";

                    await _emailService.SendEmailAsync(user.Email, subject, body);

                    return RedirectToAction("EmailConfirmationSent");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Некорректная ссылка");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            token = WebUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("EmailConfirmed");

            return BadRequest("Ошибка при подтверждении");
        }

        [HttpGet]
        public IActionResult Login(bool showModal = false)
        {
            ViewBag.ShowModal = showModal;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Неверный Email или пароль");
                    return View(model);
                }

                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Пожалуйста, подтвердите свой Email перед входом");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный Email или пароль");
                }
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> IsAuthenticated()
        {
            return Json(User.Identity.IsAuthenticated);

        }

        public async Task<IActionResult> Profile()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return View("Error");
            
            var user = await _userManager.FindByIdAsync(userId);


            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();
            return View("UserProfile", user);
        }
    }
}