using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.DTO
{
    public class CityDTO
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public State State { get; set; }
        [Required] public int StateId { get; set; }

    }
}
