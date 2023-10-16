using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.ServiceContract
{
    public interface IHotelService
    {
        Task<HotelVM> CreateHotel( HotelVM hotel);
        Task<RoomTypeDTO> AddRoomType(RoomTypeDTO roomTypeDTO);
        Task<IEnumerable<HotelDTO>> GetHotels(int cityId);
        Task<HotelVM> GetHotel(int id);


    }
}
