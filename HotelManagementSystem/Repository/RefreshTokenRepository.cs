using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
