using Bel_Souvenirs.Models;
using Bel_Souvenirs.Services;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Bel_Souvenirs.Controllers
{
    public class AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        CartService cartService,
        EmailService emailService,
        IUserService userService) : BaseController(cartService)
    {

        private readonly IUserService _userService = userService;

        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly EmailService _emailService = emailService;

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
                var result = await _userService.RegisterUserAsync(model);

                if (result.Succeeded)
                {

                    var user = await _userService.GetUserByEmailAsync(model.Email);

                    if (user is not ApplicationUser appUser)
                    {
                        ModelState.AddModelError(String.Empty, "Ошибка при получении пользователя.");
                        return View(model);
                    }

                    var token = await _userService.GenerateEmailConfirmationTokenAsync(appUser);


                    var confirmationUrl = Url.Action("ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = WebUtility.UrlEncode(token) },
                        Request.Scheme);

                    var subject = "Подтверждение Email";
                    var body =  $"<p>Здравствуйте, {user.UserName}!</p>" +
                                       $"<p>Пожалуйста, подтвердите вашу почту, перейдя по ссылке:</p>" +
                                       $"<p><a href='{confirmationUrl}'>Подтвердить email</a></p>";

                    await _emailService.SendEmailAsync(appUser.Email ?? "", subject, body);

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

            token = WebUtility.UrlDecode(token);

            var result = await _userService.ConfirmEmailAsync(userId, token);
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
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    if (isAdmin)
                    {
                        return RedirectToAction("ManageUsers", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }
              
                ModelState.AddModelError(string.Empty, "Неверный Email или пароль");
               
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

        public IActionResult IsAuthenticated()
        {
            return Json(User.Identity?.IsAuthenticated ?? false);

        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return View("Error");
            
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception ("Ошибка, пользователь не найден");

            var viewModel = new UpdateUserViewModel { 
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "Email not found"
            };

            ViewBag.CartItemCount = await _cartService.GetCartItemsCountAsync();

            return View("UserProfile", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UpdateUserViewModel model) 
        {
            if (!ModelState.IsValid) return View("UserProfile", model);

            await _userService.UpdateFullNameAsync(model.Id, model.FullName);

            TempData["SuccessMessage"] = "Имя успешно обновлено!";
            return RedirectToAction(nameof(Profile));
        }
    }
}