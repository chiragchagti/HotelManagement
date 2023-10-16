using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.DTO
{
    public class HotelDTO
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Address { get; set; }
        [Required] 
        [Display(Name = "Location Link")]
        public string LocationLink { get; set; }
        public string Images { get; set; }
        [Required] public string Amenities { get; set; }
        [Required] public int CityId { get; set; }
        [Required]
        [Display(Name = "Service Charges")]
        public int ServiceCharges { get; set; }
    }
}
