using HotelManagementSystem.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        public AccountController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender; 
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginVM loginVM)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest(ModelState);
                var user = await _userService.Authentication(loginVM);
                if (user == null)
                {
                    return BadRequest("Wrong username or password");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
          
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = await _userService.Register(register);
                if(user.IsSuccessful)
                {
                    string recipientEmail = user.User.Email;
                    string subject = "Welcome";
                    string message = "Your account created successfully.";
                    await _emailSender.SendEmailAsync(recipientEmail, subject, message);
                    return Ok(user.User);
                }
                else
                {
                    var errorsCaught = user.Errors.Select(error => error.Description).ToList();
                    return BadRequest(errorsCaught);
                }
                
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                bool result = await _userService.ForgotPassword(email);
                if (!result) return BadRequest("User not found.");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                bool result = await _userService.ResetPasswordAsync(resetPassword);
                if (!result) return BadRequest("Something Went Wrong");
                return Ok("Password Reset Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
