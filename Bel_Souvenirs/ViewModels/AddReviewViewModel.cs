using System.ComponentModel.DataAnnotations;

namespace Bel_Souvenirs.ViewModels
{
    public class AddReviewViewModel
    {
        public int ProductId { get; set; }

        [Range(1, 5, ErrorMessage = "Пожалуйста, выберите оценку от 1 до 5.")]
        public int Rating { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
