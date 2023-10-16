using HotelManagementSystem.Identity;

namespace HotelManagementSystem.Models
{
    public class Otp
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string Value { get; set; }
        public DateTime Expiration { get; set; }
    }

}
