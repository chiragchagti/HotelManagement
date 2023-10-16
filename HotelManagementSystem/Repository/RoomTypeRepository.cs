using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
