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
        [HttpGet("checkroomavailability")]
       public async Task<object> CheckRoomAvailability(DateTime startDate, DateTime endDate, int hotelRoomId, int roomsRequired, int adultGuests, int childGuests)
        {
            return await _bookingService.CheckRoomAvailability(startDate, endDate, hotelRoomId, roomsRequired, adultGuests, childGuests);
        }
    }
}
