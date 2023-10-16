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
        public IFormFileCollection files { get; set; }



    }
    public class Test {
      
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
        //public HotelDTO Hotel { get; set; }
        public List<HotelRoomDTO> HotelRooms { get; set; }

        public IFormFileCollection files { get; set; }
    }

}
