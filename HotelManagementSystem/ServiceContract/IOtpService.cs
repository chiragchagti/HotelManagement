namespace HotelManagementSystem.ServiceContract
{
    public interface IOtpService
    {
        string GenerateOtp();
        void StoreOtp(string userId, string otp);
        Task<bool> VerifyOtp(string userId, string otp);
    }

}
