using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalatisRestaurant.Models
{
    public class Product
    {
        public Product()
        {
            ProductIngredients = new List<ProductIngredient>();
        }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; } // Foreign Key Property

        [NotMapped]
        public IFormFile? ImageFile { get; set; } // For uploading images
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150"; // For storing image URL

        [ValidateNever]
        public Category? Category { get; set; } // Navigation Property

        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; } // A product can be in many order items

        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } // A product can have many ingredients
    }
}
