using System.ComponentModel.DataAnnotations;

namespace Bel_Souvenirs.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
