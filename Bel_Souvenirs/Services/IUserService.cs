
using Bel_Souvenirs.Models;
using Bel_Souvenirs.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Bel_Souvenirs.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<IdentityUser?> GetUserByEmailAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task UpdateFullNameAsync(string userId, string fullName);
    }
}
