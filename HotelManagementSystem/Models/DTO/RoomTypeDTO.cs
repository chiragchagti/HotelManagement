using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.DTO
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}
