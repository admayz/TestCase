using Microsoft.EntityFrameworkCore;
using TestCase.Models.User;
using TestCase.Repositories.Base;

namespace TestCase.Repositories
{
    public class AppRoleRepository : GenericIdRepository<AppRole>
    {
        public AppRoleRepository(DbContext context)
            : base(context)
        {
        }
    }
}
