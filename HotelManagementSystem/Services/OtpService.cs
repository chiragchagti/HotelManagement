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

        /// <summary>
        /// Generate Random Otp of 6 Digits 
        /// </summary>
        /// <returns>Otp</returns>
        public string GenerateOtp()
        {
            //Generate a Random 6 digits otp
            Random random = new Random();
            int otpValue = random.Next(100000, 999999);
            string otp = otpValue.ToString("D6"); // Format as a 6-digit string
            return otp;
        }

        /// <summary>
        /// Store the Created Otp in Database with the User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otp"></param>
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

        /// <summary>
        /// Verify otp expiry and and match with userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otp"></param>
        /// <returns>True if otp is verified</returns>
        public async Task<bool> VerifyOtp(string userId, string otp)
        {
            //verify the otp is valid or expired 
            var currentTime = DateTime.UtcNow;
            var otpRecord = await _unitOfWork.Otp.FirstOrDefaultAsync(otpInDb => otpInDb.ApplicationUserId == userId && otpInDb.Value == otp && otpInDb.Expiration > currentTime);

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
