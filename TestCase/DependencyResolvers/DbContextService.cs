using Microsoft.EntityFrameworkCore;
using TestCase.Models;

namespace TestCase.DependencyResolvers
{
    public static class DbContextService
    {
        /// <summary>
        /// Bu metod, uygulamanın veri erişim katmanını yapılandırmak için DbContext'i hizmet koleksiyonuna ekler. 
        /// SqlServer ile bağlantı kurulumu yapılır ve lazy loading proxy'leri etkinleştirilir.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddDbContextService(this IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            services.AddDbContextPool<AppData>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppConnection"))
                    .UseLazyLoadingProxies());

            return services;
        }
    }
}