using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = null!;
        public bool IsInCart { get; set; }
    }
}
