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
        /// <summary>
        /// Checks the availability of hotel rooms for a specified date range and guest count, and calculates pricing details or provides available dates.
        /// </summary>
        /// <param name="checkAvailability">A CheckAvailability object containing details of the check-in, check-out dates, and guest counts.</param>
        /// <returns>
        /// If the required number of rooms are available for all days in the specified date range, returns an object with pricing details:
        /// {
        ///     SubTotal: The total cost of the booked rooms,
        ///     ServiceCharge: The service charge based on the hotel's percentage rate.
        /// }
        /// If rooms are not available for all days, returns an object with available dates:
        /// {
        ///     AvailableDates: A dictionary with date availability information.
        /// }
        /// </returns>
        /// <exception cref="ApplicationException">Thrown if an error occurs during the availability check.</exception>
        public async Task<object> CheckRoomAvailability(CheckAvailability checkAvailability)
            {
            try
            {
                checkAvailability.StartDate = checkAvailability.StartDate.AddHours(5.5);
                checkAvailability.EndDate = checkAvailability.EndDate.AddHours(5.5);

                //Check if guests are not more than capacity.
                if (!((checkAvailability.RoomsRequired >= checkAvailability.AdultGuests / 2.0) && (checkAvailability.RoomsRequired >= checkAvailability.ChildGuests / 2.0)))
                {
                    throw new ApplicationException("Maximun 2 Adults & 2 Children allowed in one room.");
                }

                //Check if Check-in date is not in past
                if ((checkAvailability.StartDate.Date.AddHours(5.5) < DateTime.Now.Date.AddHours(5.5)) || (checkAvailability.EndDate <= checkAvailability.StartDate))
                {
                    throw new ApplicationException("Please select valid Dates.");
                }

                var availabilityByDate = new Dictionary<DateTime, bool>();
                
                //GetAll bookings of the required room between start date & end date
                var bookings = await _unitOfWork.Booking.GetAllAsync(bookings => bookings.HotelRoomId == checkAvailability.HotelRoomId && bookings.CheckInDate < checkAvailability.EndDate && bookings.CheckOutDate > checkAvailability.StartDate);
               
                //Get details about room & hotel
                var hotelRoom = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(hotelRoom => hotelRoom.Id == checkAvailability.HotelRoomId, includeProperties: "Hotel");
                
                //Total rooms available
                int totalCapacity = hotelRoom.TotalRooms; 

                //Check if required number of rooms are available for every date for which booking is required
                for (var date = checkAvailability.StartDate; date < checkAvailability.EndDate; date = date.AddDays(1))
                {
                    var totalRoomsBooked = bookings.Where(bookings => bookings.CheckInDate <= date && bookings.CheckOutDate > date).Sum(b => b.TotalRooms);
                    availabilityByDate[date] = totalCapacity - totalRoomsBooked >= checkAvailability.RoomsRequired;                    
                }
                var availabilityResult = new AvailabilityResult();
               
                //If required number of rooms are available for all required days, calculate total price and calculate service tax
                if (availabilityByDate.Values.All(value => value == true))
                {
                    availabilityResult.SubTotal = checkAvailability.RoomsRequired * hotelRoom.Price * availabilityByDate.Count();
                    availabilityResult.ServiceCharge = availabilityResult.SubTotal * hotelRoom.Hotel.ServiceCharges / 100;
                    availabilityResult.IsAvailable = true;
                    availabilityResult.AvailabilityByDate = availabilityByDate;
                    availabilityResult.SGST = availabilityResult.SubTotal * SD.SGST / 100;
                    availabilityResult.CGST = availabilityResult.SubTotal * SD.CGST / 100;
                    availabilityResult.TotalAmount = availabilityResult.SubTotal + availabilityResult.ServiceCharge + availabilityResult.SGST + availabilityResult.CGST;
                    return availabilityResult;
                }

                //if rooms are not available return rooms availability
                else 
                {
                    availabilityResult.IsAvailable = false;
                    availabilityResult.AvailabilityByDate = availabilityByDate;
                    return availabilityResult;
                }
               

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);

            }
        }


    }
}
