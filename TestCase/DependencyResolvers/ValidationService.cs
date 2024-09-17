using FluentValidation;
using FluentValidation.AspNetCore;
using TestCase.Models;
using TestCase.Profiles;
using TestCase.Validations;

namespace TestCase.DependencyResolvers
{
    public static class ValidationService
    {
        /// <summary>
        /// Uygulama için doğrulama hizmetlerini yapılandırır.
        /// Bu metod, AutoMapper'ı yapılandırır, FluentValidation'ı otomatik doğrulama ile entegre eder ve <see cref="ProductValidator"/> sınıfını <see cref="IValidator{T}"/> olarak servis koleksiyonuna ekler.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddValidationService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<Product>, ProductValidator>();

            return services;
        }
    }
}