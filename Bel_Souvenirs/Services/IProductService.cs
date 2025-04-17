using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.Services
{
    public interface IProductService
    {
        Task UpdateAmountOfOrdersAsync(int productId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetAllProducts(int offset, int count);
        Task<List<Product>> GetFilteredProductsAsync(string? search, string? category, string? sortOrder);
        Task<List<string>> GetCategoryNamesAsync();
        Task<double> GetAverageRatingAsync(int productId);
    }
}
