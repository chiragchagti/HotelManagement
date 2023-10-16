using HotelManagementSystem.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required] public string ApplicationUserId { get; set; }
        [Required]
        [Display (Name ="Check-In Date")]
        public DateTime CheckInDate { get; set; }
        [Required]
        [Display(Name = "Check-Out Date")]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public string Adults { get; set; }
        [Required]
        public string Children { get; set; }
        [Required]
        [Display (Name ="Total price without Gst")]
        public int PriceWithoutGst { get; set; }
        [Required]
        [Display(Name = "Total price with Gst")]
        public string PriceWithGst { get; set; }
        [Required]
        [Display(Name ="Total Rooms")]
        public int TotalRooms { get; set; }
        [Required]
        public string BookingStatus { get; set; }
        public HotelRoom HotelRoom { get; set; }
        [Required]
        public int HotelRoomId { get; set; }
    }
}


