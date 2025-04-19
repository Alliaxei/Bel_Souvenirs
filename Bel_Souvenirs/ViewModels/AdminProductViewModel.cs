using System.ComponentModel.DataAnnotations;

namespace Bel_Souvenirs.ViewModels
{
    public class AdminProductViewModel
    {
        [Required(ErrorMessage = "Название товара обязательно.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Бренд товара обязателен.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Модель товара обязательна.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание товара обязательно.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цена товара обязательна.")]
        [Range(1, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Категория товара обязательна.")]
        public string Category { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
