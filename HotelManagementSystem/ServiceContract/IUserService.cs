using HotelManagementSystem.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Services;

namespace HotelManagementSystem.ServiceContract
{
    public interface IUserService
    {
        Task<ApplicationUser> Authentication(LoginVM loginVM);
        Task<RegistrationResult> Register(Register register);
        Task<bool> ForgotPassword(string email);
        Task<bool> VerifyOtp(string userId, string otp);
        Task<bool> ResetPasswordAsync(ResetPassword resetPassword);

    }
}
