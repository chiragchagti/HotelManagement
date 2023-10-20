using HotelManagementSystem.Models;

namespace HotelManagementSystem.ServiceContract
{
    public interface IBookingService
    {
        Task<object> CheckRoomAvailability(CheckAvailability checkAvailability);
    }
}
