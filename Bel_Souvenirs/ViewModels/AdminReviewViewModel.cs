namespace Bel_Souvenirs.ViewModels
{
    public class AdminReviewViewModel
    {
        public int Id { get; set; }

        public string AuthorEmail { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
