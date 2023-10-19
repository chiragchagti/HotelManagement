using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.OutputDTO
{
    public class HotelRoomOutputDTO
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        [Required]
        public int HotelId { get; set; }
      
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        [Display(Name ="Total Rooms")]
        public int TotalRooms { get; set; }
        public string Images { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
