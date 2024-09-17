using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestCase.Models.User;

namespace TestCase.Models
{
    public class AppData : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppData()
        {

        }

        public AppData(DbContextOptions<AppData> options) 
            : base(options)
        {

        }

        #region Table

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<Product> Products { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
