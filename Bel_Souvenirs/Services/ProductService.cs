using Bel_Souvenirs.Models;
using Microsoft.EntityFrameworkCore;

namespace Bel_Souvenirs.Services
{

    public class ProductService(AppDbContext context) : IProductService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            return product ?? throw new KeyNotFoundException($"Продукт с ID {productId} не найден.");
        }
        public async Task UpdateAmountOfOrdersAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null) 
            {
                product.AmountOfOrders += 1;
                await _context.SaveChangesAsync();
            }
        }
    }
}
