using TestCase.Models.Initializer;
using TestCase.Models;
using Microsoft.EntityFrameworkCore;
using TestCase.Utlis;
using TestCase.Repositories.Base;

namespace TestCase.DependencyResolvers
{
    public static class InjectionService
    {
        /// <summary>
        /// Uygulamanın bağımlılıklarını servis koleksiyonuna ekler. 
        /// Bu metod, genel repository, DbContext, veritabanı başlatıcı ve sabit değer yapılandırıcıyı servis koleksiyonuna ekler.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<DbContext, AppData>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddSingleton<IConstsBuilder, ConstsBuilder>();

            return services;
        }
    }
}