using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Repository.IRepository;

namespace HotelManagementSystem.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        private readonly ApplicationDbContext _context;
        public StateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
