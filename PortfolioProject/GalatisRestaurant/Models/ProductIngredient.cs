namespace GalatisRestaurant.Models
{
    public class ProductIngredient
    {
        // Join table

        public int ProductId { get; set; } // Foreign Key Property
        public Product Product { get; set; } // Navigation Property
        public int IngredientId { get; set; } // Foreign Key Property
        public Ingredient Ingredient { get; set; } // Navigation Property
    }
}
