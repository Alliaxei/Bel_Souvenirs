using Bel_Souvenirs.Models;
using Bel_Souvenirs.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bel_Souvenirs.Services
{
    public class ProductService(AppDbContext context, IReviewService reviewService) : IProductService
    {
        private readonly AppDbContext _context = context;
        private readonly IReviewService _reviewService = reviewService;

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<List<Product>> GetAllProducts(int offset, int count)
        {
            return await _context.Products.Skip(offset).Take(count).ToListAsync();
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
        public async Task<List<Product>> GetFilteredProductsAsync(string? search, string? category, string? sortOrder)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowered = search.ToLower();
                query = query.Where(p =>
                    p.Name != null &&
                    EF.Functions.Like(p.Name.ToLower(), lowered + "%")
                    );
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category != null && p.Category ==  category);
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "popular" => query.OrderByDescending(p => p.AmountOfOrders),
                _ => query.OrderBy(p => p.Name)
            };

            return await query.ToListAsync();
        }
        public async Task<List<string>> GetCategoryNamesAsync()
        {
            return await _context.Products
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var reviews = await _reviewService.GetAllReviewsAsync(productId);

            return reviews.Count != 0 ? reviews.Average(r => r.Rating) : 0;
        }
        public async Task AddProductAsync(AdminProductViewModel model, string imagePath)
        {
            var product = new Product
            {
                Name = model.Name,
                Brand = model.Brand,
                Model = model.Model,
                Rating = 0,
                Description = model.Description,
                Price = model.Price,
                ImagePath = imagePath,
                Category = model.Category,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(int id, AdminProductViewModel model, string? imagePath)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new Exception("Товар не найден");
            
            product.Name = model.Name;
            product.Brand = model.Brand;
            product.Model = model.Model;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Category = model.Category;

            if (!string.IsNullOrEmpty(imagePath)) product.ImagePath = imagePath;
            

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new Exception ("Товар не найден");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}