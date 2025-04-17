namespace GalatisRestaurant.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; } // Foreign Key Property
        public ApplicationUser User { get; set; } // Navigation Property
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } // Each Order has a list of order items
    }
}
