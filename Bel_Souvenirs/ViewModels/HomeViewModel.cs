namespace Bel_Souvenirs.ViewModels
{
    public class HomeViewModel
    {
        public List<ProductViewModel> AllProducts { get; set; } = [];
        public List<ProductViewModel> TopPopularProducts { get; set; } = [];
    }
}
