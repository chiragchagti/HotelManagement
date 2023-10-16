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
    //[Authorize]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHotelService _hotelService;

        public HotelController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHotelService hotelService)
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
        public async Task<IActionResult> CreateHotel([FromForm] HotelVM hotel)
        {
            try
            {
                if (hotel == null) return BadRequest();
                var hotelVM = await _hotelService.CreateHotel(hotel);
                if (hotelVM == null) return NotFound();
                return Ok(hotelVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }


        }
        [HttpPost]
        public async Task<IActionResult> test([FromForm] Test hotel)
        {
            return Ok(hotel);
        }
    }
}