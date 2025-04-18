namespace Bel_Souvenirs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public int AmountOfOrders { get; set; } = 0;
        public bool IsPopular { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual List<CartItem> CartItems { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
    }
}
