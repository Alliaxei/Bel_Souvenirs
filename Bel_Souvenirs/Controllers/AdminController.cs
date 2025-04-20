using Bel_Souvenirs.ViewModels;
using Bel_Souvenirs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bel_Souvenirs.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Runtime.CompilerServices;


namespace Bel_Souvenirs.Controllers
{
    public class AdminController(UserManager<ApplicationUser> userManager, IProductService productService, IWebHostEnvironment webHostEnvironment,
        IUserService userService, IReviewService reviewService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IProductService _productService = productService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IUserService _userService = userService;
        private readonly IReviewService _reviewService = reviewService;

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return RedirectToAction("ManageUsers", "Admin");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin")) continue;

                var userViewModel = new AdminUserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? "",
                    Role = roles.Count > 0 ? roles[0] : "User"
                };
                userViewModels.Add(userViewModel);
            }
            var model = new AdminIndexViewModel
            {
                Users = userViewModels
            };

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageProducts()
        {
            var products = await _productService.GetAllProducts();
            return View(products);
        }

        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> ManageReviews()
        {
            var reviews = await _reviewService.GetAllReviewsViewModelsAsync();

            return View(reviews);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AdminProductViewModel modelView)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Пожалуйста, заполните все поля корректно.";
                return RedirectToAction("ManageProducts");
            }

            string imagePath = string.Empty;

            if (modelView.ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(modelView.ImageFile.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await modelView.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/products/" + fileName;
            }

            await _productService.AddProductAsync(modelView, imagePath);
            TempData["SuccessMessage"] = "Товар успешно добавлен!";

            return RedirectToAction("ManageProducts");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(int id, AdminProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Пожалуйста, заполните все поля корректно.";
                return RedirectToAction("ManageProducts");
            }

            string imagePath = string.Empty;
            
            if (viewModel.ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(viewModel.ImageFile.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/image/products/" + fileName;
            }
            
            try
            {
                await _productService.UpdateProductAsync(id, viewModel, imagePath);
                TempData["SuccessMessage"] = "Товар успешно обновлён!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при редактировании товара " + ex.Message;
            }

            return RedirectToAction("ManageProducts");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser([FromForm] string id, [FromForm] string fullName)
        {
         

            try
            {
                await _userService.UpdateFullNameAsync(id, fullName);
                TempData[key: "SuccessMessage"] = "Пользователь успешно обновлён";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при редактировании пользователя " + ex.Message;
            }

            return RedirectToAction("ManageUsers");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                TempData[key: "SuccessMessage"] = "Пользователь успешно удалён";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при удалении пользователя " + ex.Message;
            }

            return RedirectToAction("ManageUsers");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                TempData[key: "SuccessMessage"] = "Товар успешно удалён";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при удалении товара " + ex.Message;
            }

            return RedirectToAction("ManageProducts");

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (result)
            {
                TempData[key: "SuccessMessage"] = "Комментарий успешно удалён";
                return RedirectToAction("ManageReviews");
            }
            TempData["ErrorMessage"] = "Ошибка при удалении комментария ";
            return RedirectToAction("ManageReviews");
        }

    }
}
