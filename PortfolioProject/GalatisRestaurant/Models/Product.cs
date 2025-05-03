using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "A product name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "A description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "A price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stock is required")]
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

        //Read-only property for the slug
        public string Slug => Name!.Replace(' ', '-').ToLower();
    }
}
