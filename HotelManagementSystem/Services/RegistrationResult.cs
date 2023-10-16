using HotelManagementSystem.Identity;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Services
{

    public class RegistrationResult
    {
        public bool IsSuccessful { get; set; }
        public ApplicationUser User { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }

}
