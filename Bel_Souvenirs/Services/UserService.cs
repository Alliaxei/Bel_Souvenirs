﻿using Bel_Souvenirs.Models;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Bel_Souvenirs.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public UserService(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                var cart = new Cart
                {
                    UserId = user.Id,
                    Items = []
                };

                _appDbContext.Carts.Add(cart);
                await _appDbContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
            
        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebUtility.UrlEncode(token);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Пользователь не найден" });

            var decodedtoken = WebUtility.UrlDecode(token);
            return await _userManager.ConfirmEmailAsync(user, decodedtoken);
        }
    }
}