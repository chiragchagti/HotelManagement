using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly ApplicationDbContext _context;
        public CityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
