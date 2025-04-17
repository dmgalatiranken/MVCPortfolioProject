namespace GalatisRestaurant.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public ICollection<ProductIngredient> ProductIngredients { get; set; } // An ingredient can be used in many products
    }
}
