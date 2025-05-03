using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalatisRestaurant.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }

        [NotMapped]
        public IList<string> RoleNames { get; set; } = null!;
    }
}
