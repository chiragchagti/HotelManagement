using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class OtpRepository : Repository<Otp>, IOtpRepository
    {
        private readonly ApplicationDbContext _context;
        public OtpRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
