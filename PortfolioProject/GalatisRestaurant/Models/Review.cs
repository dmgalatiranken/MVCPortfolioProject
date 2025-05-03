using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GalatisRestaurant.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "A rating is required.")]
        public int Rating { get; set; } // 1 to 5 in stars
        [Required(ErrorMessage = "A comment is required.")]
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now; // Review date defaults to now
    }
}
