using TestCase.Models.User;

namespace TestCase.Models.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppData context;

        public DbInitializer(AppData context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any(d => d.Name == "admin"))
            {
                var roleToChoose = new AppRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };
                context.Roles.Add(roleToChoose);
            }
            if (!context.Roles.Any(d => d.Name == "user"))
            {
                var roleToChoose = new AppRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                };
                context.Roles.Add(roleToChoose);
            }

            context.SaveChanges();
        }
    }
}
