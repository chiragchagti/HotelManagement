using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.ServiceContract
{
    public interface IHotelService
    {
        Task<HotelDTO> CreateHotel( HotelDTO hotelDTO);
        Task<RoomTypeDTO> AddRoomType(RoomTypeDTO roomTypeDTO);
        Task<IEnumerable<HotelDTO>> GetHotels(int cityId);
        Task<HotelVM> GetHotel(int id);
        Task<ICollection<HotelRoomDTO>> AddRoomsInHotel(ICollection<HotelRoomDTO> hotelRoomDTO);
        Task<ICollection<HotelRoomDTO>> UpdateRoomsInHotel(ICollection<HotelRoomDTO> hotelRoomDTO);
        Task<HotelRoomDTO> UpdateRoomInHotel(HotelRoomDTO hotelRoomDTO);


    }
}
