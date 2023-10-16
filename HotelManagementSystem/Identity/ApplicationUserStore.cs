using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelManagementSystem.Identity
{
  public class ApplicationUserStore: UserStore<ApplicationUser>
  {
    public ApplicationUserStore(ApplicationDbContext context)
      : base(context)
    {

    }
  }
}
