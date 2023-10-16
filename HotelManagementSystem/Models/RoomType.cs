using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}
