namespace HotelManagementSystem.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHotelRepository Hotel {  get; }
        IHotelRoomRepository HotelRoom { get; }
        IRoomTypeRepository RoomType { get; }
        IBookingRepository Booking { get; }
        IPaymentRepository Payment { get; }
        IStateRepository State { get; }
        ICityRepository City { get; }
        IOtpRepository Otp { get; }
        IRefreshTokenRepository RefreshToken { get; }

        void Save();
    }
}
