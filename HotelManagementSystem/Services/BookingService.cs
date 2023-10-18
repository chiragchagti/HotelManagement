using AutoMapper;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Services
{
    public class BookingService: IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //public async Task<Dictionary<DateTime, bool>> CalculateRoomAvailabilityAsync(DateTime startDate, DateTime endDate, int hotelRoomId, int roomsRequired)
        //{
        //    var availabilityByDate = new Dictionary<DateTime, bool>();
        //    var bookings = await _unitOfWork.Booking.GetAllAsync(b => b.HotelRoomId == hotelRoomId && b.CheckInDate < endDate && b.CheckOutDate > startDate, includeProperties: "Hotel");
        //    var hotelRoom = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(hr => hr.Id == hotelRoomId);
        //    int totalCapacity = hotelRoom.TotalRooms;
        //    for (var date = startDate; date < endDate; date = date.AddDays(1))
        //    {
        //        var bookingsOnDate = bookings.Where(b => b.CheckInDate <= date && b.CheckOutDate > date).ToList();
        //        int sumBookedRooms = bookingsOnDate.Sum(b => b.TotalRooms);

        //        availabilityByDate[date] = totalCapacity - sumBookedRooms >= roomsRequired;

        //        availabilityByDate[date] = totalCapacity - sumBookedRooms >= roomsRequired;
        //    }


        //    return availabilityByDate;
        //}

        public async Task<object> CheckRoomAvailability(DateTime startDate, DateTime endDate, int hotelRoomId, int roomsRequired, int adultGuests, int childGuests)
            {
            try
            {
                if (!((roomsRequired >= adultGuests / 2.0) && (roomsRequired >= childGuests / 2.0)))
                {
                    throw new ApplicationException("Maximun 2 Adults & 2 Children allowed in one room.");
                }
                if (startDate.Date < DateTime.Now.Date)
                {
                    throw new ApplicationException("Please select valid Dates.");
                }
                var availabilityByDate = new Dictionary<DateTime, bool>();
                var bookings = await _unitOfWork.Booking.GetAllAsync(b => b.HotelRoomId == hotelRoomId && b.CheckInDate < endDate && b.CheckOutDate > startDate);
                var hotelRoom = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(hr => hr.Id == hotelRoomId, includeProperties: "Hotel");
                int totalCapacity = hotelRoom.TotalRooms;
                for (var date = startDate; date < endDate; date = date.AddDays(1))
                {
                    var totalRoomsBooked = bookings.Where(b => b.CheckInDate <= date && b.CheckOutDate > date).Sum(b => b.TotalRooms);
                   // int sumBookedRooms = bookingsOnDate.Sum(b => b.TotalRooms);

                    availabilityByDate[date] = totalCapacity - totalRoomsBooked >= roomsRequired;

                    
                }
                if (availabilityByDate.Values.All(value => value == true))
                {
                    int subTotal = roomsRequired * hotelRoom.Price * availabilityByDate.Count();
                    int serviceCharge = subTotal * hotelRoom.Hotel.ServiceCharges / 100;
                    var result = new
                    {
                        SubTotal = subTotal,
                        ServiceCharge = serviceCharge
                    };
                    return result;
                }
                else 
                {
                     var result = new
                {
                    AvailableDates = availabilityByDate,
                };
                return result;
                }
               

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);

            }
        }


    }
}
