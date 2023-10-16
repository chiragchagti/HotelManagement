
using HotelManagementSystem.Identity;
using HotelManagementSystem.Migrations;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ApplicationRoleManager> _logger;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IOtpService _otpService;

        public UserService(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager,
            UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings,
            IUnitOfWork unitOfWork,
            ILogger<ApplicationRoleManager> logger,
            IUserStore<ApplicationUser> userStore,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender,
            IOtpService otpService
            )
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userStore = userStore;
            _roleManager = roleManager;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _otpService = otpService;

        }

        public async Task<ApplicationUser> Authentication(LoginVM loginVM)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(loginVM.UserName);
                //JWT Authentication
                if (await _userManager.IsInRoleAsync(applicationUser, SD.Role_SuperAdmin))
                    applicationUser.Role = SD.Role_SuperAdmin;
                if (await _userManager.IsInRoleAsync(applicationUser, SD.Role_Admin))
                    applicationUser.Role = SD.Role_Admin;
                if (await _userManager.IsInRoleAsync(applicationUser, SD.Role_Receptionist))
                    applicationUser.Role = SD.Role_Receptionist;
                if (await _userManager.IsInRoleAsync(applicationUser, SD.Role_User))
                    applicationUser.Role = SD.Role_User;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescritor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, applicationUser.Id),
                    new Claim(ClaimTypes.Email, applicationUser.Email),
                    new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescritor);
                applicationUser.Token = tokenHandler.WriteToken(token);
                //**
                var refreshToken = GenerateRefreshToken(applicationUser.Id);
                await _unitOfWork.RefreshToken.AddAsync(refreshToken);
                applicationUser.PasswordHash = "";

                return applicationUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<RegistrationResult> Register(Register register)
        {
            var user = new ApplicationUser()
            {
                Name = register.Name,
                Email = register.Email,
                UserName = register.Email,
                PhoneNumber = register.PhoneNumber,
                Role = register.Role,
            };

            await _userStore.SetUserNameAsync(user, register.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, register.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

  
                if (!await _roleManager.RoleExistsAsync(SD.Role_SuperAdmin))
                {
                    var role = new ApplicationRole();
                    role.Name = SD.Role_SuperAdmin;
                    await _roleManager.CreateAsync(role);
                }

                if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                {
                    var role = new ApplicationRole();
                    role.Name = SD.Role_Admin;
                    await _roleManager.CreateAsync(role);
                }
                if (!await _roleManager.RoleExistsAsync(SD.Role_Receptionist))
                {
                    var role = new ApplicationRole();
                    role.Name = SD.Role_Receptionist;
                    await _roleManager.CreateAsync(role);
                }
                if (!await _roleManager.RoleExistsAsync(SD.Role_User))
                {
                    var role = new ApplicationRole();
                    role.Name = SD.Role_User;
                    await _roleManager.CreateAsync(role);
                }

                // await _userManager.AddToRoleAsync(user, SD.Role_Admin);
                if (register.Role == null)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_User);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, register.Role);
                }
                // Return a success result
                return new RegistrationResult { IsSuccessful = true, User = user };
            }
            else
            {
                // If the registration was not successful, return the errors
                return new RegistrationResult { IsSuccessful = false, Errors = result.Errors };
            }
        }
        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Handle the case where the email address is not found (e.g., return an error message).
                return false;
            }


            string otp = _otpService.GenerateOtp();

            // Store OTP and associate it with the user
            _otpService.StoreOtp(user.Id, otp);


            //var resetLink = $"https://example.com/resetpassword?email={email}&code={resetCode}";


            string recipientEmail = email;
            string subject = "Reset Password";
            string message = "Your password Reset code is:"+ otp;
            await _emailSender.SendEmailAsync(recipientEmail, subject, message);

            return true;
        }
        public async Task<bool> VerifyOtp(string userId, string otp)
        {
            var result =await _otpService.VerifyOtp(userId, otp);
            return result;
        }
        public async Task<bool> ResetPasswordAsync(ResetPassword resetPassword)
        {
            // Verify the OTP
            bool isOtpValid = await _otpService.VerifyOtp(resetPassword.UserId, resetPassword.Otp);
            if (!isOtpValid)
            {
                return false; // Invalid OTP
            }

            // Reset the password
            var user = await _userManager.FindByIdAsync(resetPassword.UserId);
            if (user == null)
            {
                return false; // User not found
            }

            // Update the password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);

            if (result.Succeeded)
            {
                // Password reset was successful
                return true;
            }

            // Password reset failed
            return false;
        }
        public RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                ApplicationUserId = userId,
                Token = Convert.ToBase64String(randomNumber),
                Expiration = DateTime.UtcNow.AddMonths(1), // Set the expiration as desired
            };
        }
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

    }
}



