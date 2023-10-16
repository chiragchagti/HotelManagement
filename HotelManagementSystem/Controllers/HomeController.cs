using AutoMapper;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHotelService _hotelService;
        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, IHotelService hotelService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelService = hotelService;
        }

        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var states = await _unitOfWork.State.GetAllAsync();
                if (states == null)
                {
                    return NotFound();
                }
                var statesDTO = _mapper.Map<List<StateDTO>>(states);
                return Ok(statesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _unitOfWork.City.GetAllAsync();
                if (cities == null)
                {
                    return NotFound();
                }
                var citiesDTO = _mapper.Map<List<CityDTO>>(cities);
                return Ok(citiesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request. Error is:" + ex);
            }
        }




        [HttpGet("{cityId:int}" + "/hotels")]
        public async Task<IActionResult> GetHotels(int cityId)
        {
            return Ok(await _hotelService.GetHotels(cityId));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            return Ok(await _hotelService.GetHotel(id));
        }
    }
}
