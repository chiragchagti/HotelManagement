namespace HotelManagementSystem.ServiceContract
{
    public interface IBookingService
    {
        Task<object> CheckRoomAvailability(DateTime startDate, DateTime endDate, int hotelRoomId, int roomsRequired, int adultGuests, int childGuests);
    }
}
