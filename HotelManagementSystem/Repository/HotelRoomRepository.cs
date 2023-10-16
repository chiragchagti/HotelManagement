using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class HotelRoomRepository : Repository<HotelRoom>, IHotelRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
