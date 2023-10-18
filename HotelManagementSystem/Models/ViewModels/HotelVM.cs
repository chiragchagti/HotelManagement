using HotelManagementSystem.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.ViewModels
{

    public class HotelVM
    {
        public HotelVM()
        {
            HotelRooms = new List<HotelRoomDTO>();
        }
        public HotelDTO Hotel { get; set; }
        public List<HotelRoomDTO> HotelRooms { get; set; }
    }
}
