using System.ComponentModel.DataAnnotations;

namespace Bel_Souvenirs.ViewModels
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string Text { get; set; } = string.Empty;
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
