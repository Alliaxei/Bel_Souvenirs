namespace Bel_Souvenirs.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual List<CartItem> Items { get; set; } = [];
    }
}
