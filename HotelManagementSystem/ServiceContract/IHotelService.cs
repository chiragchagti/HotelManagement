﻿using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Models.OutputDTO;
using HotelManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.ServiceContract
{
    public interface IHotelService
    {
        Task<HotelDTO> CreateHotel( HotelDTO hotelDTO);
        Task<RoomTypeDTO> AddRoomType(RoomTypeDTO roomTypeDTO);
        Task<IEnumerable<HotelDTO>> GetHotels(int cityId);
        Task<HotelDTO> GetHotel(int id);
        Task<ICollection<HotelRoomDTO>> AddRoomsInHotel(ICollection<HotelRoomDTO> hotelRoomDTO);
        Task<HotelRoomDTO> UpdateRoomInHotel(HotelRoomDTO hotelRoomDTO);
        Task<HotelRoomDTO> GetHotelRoom(int id);

    }
}
