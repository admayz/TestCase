using Microsoft.AspNetCore.Identity;
using TestCase.Models.User;
using TestCase.Models;
using TestCase.Utlis;

namespace TestCase.DependencyResolvers
{
    public static class IdentityServerService
    {
        /// <summary>
        /// Bu metod, ASP.NET Core uygulamasına kimlik doğrulama ve yetkilendirme hizmetlerini ekler.
        /// IdentityServer ve ASP.NET Identity yapılandırmasını gerçekleştirir ve varsayılan ayarları uygular.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(x =>
            {
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequiredLength = 3;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
            })
              .AddEntityFrameworkStores<AppData>()
              .AddDefaultTokenProviders();

            var builderIdentity = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
              .AddInMemoryIdentityResources(Config.IdentityResources)
              .AddInMemoryApiResources(Config.ApiResources)
              .AddInMemoryApiScopes(Config.ApiScopes)
              .AddInMemoryClients(Config.Clients)
              .AddAspNetIdentity<AppUser>();

            builderIdentity.AddDeveloperSigningCredential();

            return services;
        }

        /// <summary>
        /// Bu metod, uygulamanın başlangıç aşamasında kimlik doğrulama ve yetkilendirme ile ilgili varsayılan verileri oluşturur.
        /// Varsayılan roller ve kullanıcıları oluşturur ve başlangıç yapılandırmasını tamamlar.
        /// </summary>
        /// <param name="provider">Uygulamanın servis sağlayıcısı.</param>
        /// <returns>Yapılandırılmış IServiceProvider.</returns>
        public static IServiceProvider AddIdentityProvider(this IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var cs = services.GetRequiredService<IConstsBuilder>();
                cs.Initialize();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                CreateDefaultRoles(roleManager).Wait();
                CreateDefaultAdminUser(userManager, roleManager).Wait();
                CreateDefaultUser(userManager, roleManager).Wait();
            }

            return provider;
        }

        /// <summary>
        /// Varsayılan rollerin oluşturulmasını sağlar. Eğer roller mevcut değilse, yeni roller oluşturulur.
        /// </summary>
        /// <param name="roleManager">RoleManager hizmeti.</param>
        /// <returns>Görev tamamlanana kadar bekler.</returns>
        private static async Task CreateDefaultRoles(RoleManager<AppRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }
        }

        /// <summary>
        /// Varsayılan bir yönetici kullanıcısının oluşturulmasını sağlar. Eğer kullanıcı mevcut değilse, yeni bir kullanıcı oluşturulur ve yöneticilik rolü atanır.
        /// </summary>
        /// <param name="userManager">UserManager hizmeti.</param>
        /// <param name="roleManager">RoleManager hizmeti.</param>
        /// <returns>Görev tamamlanana kadar bekler.</returns>
        private static async Task CreateDefaultAdminUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var email = "admin@gmail.com";
            var adminRole = "Admin";

            if (userManager.Users.All(u => u.Email != email))
            {
                var user = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(user, "admin123*");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                if (!await userManager.IsInRoleAsync(user, adminRole))
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }

        /// <summary>
        /// Varsayılan bir kullanıcı oluşturur. Eğer kullanıcı mevcut değilse, yeni bir kullanıcı oluşturulur ve kullanıcı rolü atanır.
        /// </summary>
        /// <param name="userManager">UserManager hizmeti.</param>
        /// <param name="roleManager">RoleManager hizmeti.</param>
        /// <returns>Görev tamamlanana kadar bekler.</returns>
        private static async Task CreateDefaultUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var email = "user@gmail.com";
            var userRole = "User";

            if (userManager.Users.All(u => u.Email != email))
            {
                var user = new AppUser
                {
                    FirstName = "User",
                    LastName = "User",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "user123*");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userRole);
                }
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                if (!await userManager.IsInRoleAsync(user, userRole))
                {
                    await userManager.AddToRoleAsync(user, userRole);
                }
            }
        }
    }
}