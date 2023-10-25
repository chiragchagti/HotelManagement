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
        private readonly IHotelService _hotelService;
        public BookingController(IBookingService bookingService, IHotelService hotelService)
        {
            _bookingService = bookingService;
            _hotelService = hotelService;
        }
        [HttpPost("checkroomavailability")]
       public async Task<object> CheckRoomAvailability(CheckAvailability checkAvailability)
        {
            return await _bookingService.CheckRoomAvailability(checkAvailability);
        }

        [HttpGet("gethotelroom/"+ "{id:int}")]
        public async Task<IActionResult> GetHotelRoom(int id)
        {
            var hotelRoomDto = await _hotelService.GetHotelRoom(id);
            return Ok(hotelRoomDto);
        }
    }
}
