﻿namespace Bel_Souvenirs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = null!;
        public string Category { get; set; } = null!;



        public bool IsPopular { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual List<CartItem> CartItems { get; set; } = null!;
    }
}
