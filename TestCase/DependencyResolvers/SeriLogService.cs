using Serilog;

namespace TestCase.DependencyResolvers
{
    public static class SeriLogService
    {
        /// <summary>
        /// Uygulama için SeriLog konfigürasyonunu ve kayıt yapılandırmasını yapar.
        /// Bu metod, SeriLog'un yapılandırmasını uygulamanın ayarlarından alır ve bir logger oluşturur.
        /// Ayrıca, uygulamanın başlangıçta loglama işlemini başlatır.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddSeriLogService(this IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("Starting up");

            return services;
        }
    }
}