﻿using Bel_Souvenirs.Models;
using Bel_Souvenirs.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bel_Souvenirs.Services
{
    public class ReviewService(AppDbContext context) : IReviewService
    {
        private readonly AppDbContext _context = context;

        public async Task AddReviewAsync(int productId, string userId, string text, int rating)
        {
            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Text = text,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<Review>> GetAllReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AdminReviewViewModel>> GetAllReviewsViewModelsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new AdminReviewViewModel
                {
                    Id = r.Id,
                    AuthorEmail = r.User.Email ?? "",
                    ProductName = r.Product.Name,
                    Text = r.Text,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                })
                .ToListAsync();
        }
    }
}
