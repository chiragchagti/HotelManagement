using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Hotel = new HotelRepository(_context);
            Booking = new BookingRepository(_context);
            Payment = new PaymentRepository(_context);
            HotelRoom = new HotelRoomRepository(_context);
            RoomType = new RoomTypeRepository(_context);
            State = new StateRepository(_context);
            City = new CityRepository(_context);
            Otp = new OtpRepository(_context);
            RefreshToken = new RefreshTokenRepository(_context);
        }
        public IHotelRepository Hotel { private set; get; }
        public IBookingRepository Booking { private set; get; }
        public IPaymentRepository Payment { private set; get; }
        public IHotelRoomRepository HotelRoom { private set; get; }
        public IRoomTypeRepository RoomType { private set; get; }
        public IStateRepository State { private set; get; }
        public ICityRepository City { private set; get; }
        public IOtpRepository Otp { private set; get; }
        public IRefreshTokenRepository RefreshToken { private set; get; }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
