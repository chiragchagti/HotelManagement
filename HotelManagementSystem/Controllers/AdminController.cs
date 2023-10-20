using AutoMapper;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Repository;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHotelService _hotelService;

        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHotelService hotelService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _hotelService = hotelService;
        }
        [HttpGet("{cityId:int}" + "/hotels")]
        public async Task<IActionResult> GetHotels(int cityId)
        {
            return Ok(await _hotelService.GetHotels(cityId));
        }
        [HttpPost("addroomtype")]
        public async Task<IActionResult> AddRoomType([FromBody] RoomTypeDTO roomTypeDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (roomTypeDTO == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest();
                var roomType = await _hotelService.AddRoomType(roomTypeDTO);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }
        }
        [HttpPost("createhotel")]
        public async Task<IActionResult> CreateHotel([FromForm] HotelDTO hotelDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (hotelDTO == null) return BadRequest();
                var hotel = await _hotelService.CreateHotel(hotelDTO);
                if (hotel == null) return NotFound();
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }
        }

        [HttpPost("addroomsinhotel")]
        public async Task<IActionResult> AddRoomsInHotel([FromForm] ICollection<HotelRoomDTO> hotelRoomDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (hotelRoomDTO.Count == 0) return BadRequest();
                var hotelRooms = await _hotelService.AddRoomsInHotel(hotelRoomDTO);
                if (hotelRooms == null) return NotFound();
                return Ok(hotelRooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }
        }
        [HttpPut("updateroominhotel")]

        public async Task<IActionResult> UpdateRoomInHotel([FromForm] HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (hotelRoomDTO == null) return BadRequest();
                var hotelRoom = await _hotelService.UpdateRoomInHotel(hotelRoomDTO);
                if (hotelRoom == null) return NotFound();
                return Ok(hotelRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }

        }
    }
}