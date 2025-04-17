namespace GalatisRestaurant.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; } // Foreign Key Property
        public Category? Category { get; set; } // Navigation Property
        public ICollection<OrderItem>? OrderItems { get; set; } // A product can be in many order items
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } // A product can have many ingredients
    }
}
