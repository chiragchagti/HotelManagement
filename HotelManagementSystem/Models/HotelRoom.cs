using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class HotelRoom
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        [Required]
        public int HotelId { get; set; }
        public RoomType RoomType { get; set; }
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        [Display(Name ="Total Rooms")]
        public int TotalRooms { get; set; }
        public string Images { get; set; }
    }
}
