using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Services
{
    public class OtpService: IOtpService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OtpService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string GenerateOtp()
        {
            Random random = new Random();
            int otpValue = random.Next(100000, 999999);
            string otp = otpValue.ToString("D6"); // Format as a 6-digit string
            return otp;
        }

        public void StoreOtp(string userId, string otp)
        {
            // Store the OTP associated with the user

            var otpRecord = new Otp
            {
                ApplicationUserId = userId,
                Value = otp,
                Expiration = DateTime.UtcNow.AddMinutes(5) // Set an expiration time (e.g., 15 minutes)
            };

            _unitOfWork.Otp.AddAsync(otpRecord);
            
        }

        public async Task<bool> VerifyOtp(string userId, string otp)
        {
            var currentTime = DateTime.UtcNow;
            var otpRecord = await _unitOfWork.Otp.FirstOrDefaultAsync(o => o.ApplicationUserId == userId && o.Value == otp && o.Expiration > currentTime);

            if (otpRecord != null)
            {
              await  _unitOfWork.Otp.RemoveAsync(otpRecord.Id);
                _unitOfWork.Save();
                return  true;
            }

            return false;
        }
    }
}
