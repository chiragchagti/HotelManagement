using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelManagementSystem.Identity
{
  public class ApplicationRoleStore :
    RoleStore<ApplicationRole, ApplicationDbContext>
  {
    public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber)
    {

    }
  }
}
