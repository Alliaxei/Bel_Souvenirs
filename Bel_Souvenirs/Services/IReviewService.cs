using Bel_Souvenirs.Models;

namespace Bel_Souvenirs.Services
{
    public interface IReviewService
    {
        public Task AddReviewAasync(string userId, int productId, string text, int rating);
        public Task<List<Review>> GetAllReviewsAsync(int productId);
    }
}
