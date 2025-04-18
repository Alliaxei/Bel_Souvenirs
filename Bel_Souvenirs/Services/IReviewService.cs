using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.Services
{
    public interface IReviewService
    {
        public Task<List<Review>> GetAllReviewsAsync(int productId);
        public Task AddReviewAsync(int productId, string userId, string text, int rating);
        public Task<bool> DeleteReviewAsync(int reviewId);
    }
}
