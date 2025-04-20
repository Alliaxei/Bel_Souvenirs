using Bel_Souvenirs.Models;
using Bel_Souvenirs.ViewModels;

namespace Bel_Souvenirs.Services
{
    public interface IReviewService
    {
        public Task<List<Review>> GetAllReviewsAsync(int productId);
        public Task AddReviewAsync(int productId, string userId, string text, int rating);
        public Task<bool> DeleteReviewAsync(int reviewId);
        public Task<List<AdminReviewViewModel>> GetAllReviewsViewModelsAsync();
    }
}
