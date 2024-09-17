using Microsoft.EntityFrameworkCore;
using TestCase.Models;
using TestCase.Models.User;
using TestCase.Repositories.Base;

namespace TestCase.Repositories
{
    public class AppUserRepository : GenericIdRepository<AppUser>
    {
        public AppUserRepository()
           : base(new AppData())
        {
        }

        public AppUserRepository(DbContext context)
            : base(context)
        {
        }

        public IQueryable<AppUser> GetUserList()
        {
            return Conn.AppUsers;
        }
    }
}
