namespace HotelManagementSystem.Models
{
    public class CheckAvailability
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HotelRoomId { get; set; }
        public int RoomsRequired { get; set; }
        public int AdultGuests { get; set; }
        public int ChildGuests { get; set; }
    }
}
