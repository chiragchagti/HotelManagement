using HotelManagementSystem.Models;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost("checkroomavailability")]
       public async Task<object> CheckRoomAvailability(CheckAvailability checkAvailability)
        {
            return await _bookingService.CheckRoomAvailability(checkAvailability);
        }
    }
}
