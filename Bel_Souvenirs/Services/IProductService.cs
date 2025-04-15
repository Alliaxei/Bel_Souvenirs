using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.Services
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int productId);
        Task UpdateAmountOfOrdersAsync(int productId);
        Task<List<Product>> GetAllProducts();
    }
}
