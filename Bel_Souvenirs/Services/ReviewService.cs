using Bel_Souvenirs.Models;
using Microsoft.EntityFrameworkCore;

namespace Bel_Souvenirs.Services
{
    public class ReviewService(AppDbContext context) : IReviewService
    {
        private readonly AppDbContext _context = context;

        public Task AddReviewAasync(string userId, int productId, string text, int rating)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Review>> GetAllReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
