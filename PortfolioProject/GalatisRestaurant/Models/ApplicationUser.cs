using Microsoft.AspNetCore.Identity;

namespace GalatisRestaurant.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
