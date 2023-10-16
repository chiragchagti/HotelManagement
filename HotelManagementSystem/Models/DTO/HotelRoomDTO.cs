using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.DTO
{
    public class HotelRoomDTO
    {
        public int Id { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        [Display(Name ="Total Rooms")]
        public int TotalRooms { get; set; }
        [Required]
        public string Images { get; set; }
    }
}
