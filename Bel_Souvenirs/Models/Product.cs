namespace Bel_Souvenirs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public double Rating { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Category { get; set; } = null!;

        public int AmountOfOrders { get; set; } = 0;
        public bool IsPopular { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual List<CartItem> CartItems { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = [];
    }
}
