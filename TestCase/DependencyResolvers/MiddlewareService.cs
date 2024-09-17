using TestCase.Middleware;

namespace TestCase.DependencyResolvers
{
    public static class MiddlewareService
    {
        /// <summary>
        /// Uygulamanın orta katmanına (middleware) özel bir işleyici ekler.
        /// Bu metod, uygulamanın hata işleme işlemlerini gerçekleştiren özel bir middleware olan <see cref="ExceptionMiddleware"/>'i ekler.
        /// </summary>
        /// <param name="builder">Uygulama yapılandırması için kullanılan <see cref="IApplicationBuilder"/> örneği.</param>
        /// <returns>Yapılandırılmış <see cref="IApplicationBuilder"/> örneği.</returns>
        public static IApplicationBuilder AddMiddlewareService(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
            
            return builder;
        }
    }
}