namespace Bel_Souvenirs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string Category { get; set; }
        public bool IsPopular { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public List<CartItem> CartItems { get; set; }
    }
}
