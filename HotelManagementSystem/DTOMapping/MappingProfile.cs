using HotelManagementSystem.Models;
using HotelManagementSystem.Models.DTO;
using AutoMapper;

namespace HotelManagementSystem.DTOMapping
{
  public class MappingProfile:Profile
  {
    public MappingProfile()
    {
      CreateMap<Hotel, HotelDTO>().ReverseMap();
      CreateMap<State, StateDTO>().ReverseMap();
      CreateMap<City,CityDTO>().ReverseMap();
      CreateMap<Register, RegisterDTO>().ReverseMap();
      CreateMap<RoomType, RoomTypeDTO>().ReverseMap();
      CreateMap<HotelRoom, HotelRoomDTO>().ReverseMap();
      CreateMap<Payment, PaymentDTO>().ReverseMap();
      CreateMap<Booking, BookingDTO>().ReverseMap();
        }
    }
}
