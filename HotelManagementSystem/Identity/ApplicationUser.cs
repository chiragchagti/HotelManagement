using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
