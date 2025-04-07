using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.ViewModels
{
    public class ProductViewModel
    {
        public Product product { get; set; } = null!;
        public bool isInCart { get; set; }
    }
}
