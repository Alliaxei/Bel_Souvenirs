using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = null!;
        public bool IsInCart { get; set; }
        public List<Review> Reviews { get; set; } = [];
        public string NewComment { get; set; } = string.Empty;
        public int NewRating { get; set; }
        public int Rating { get; set; }
        public double AverageRating { get; set; }
    }
}
