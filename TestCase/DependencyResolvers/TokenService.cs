using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestCase.DependencyResolvers
{
    public static class TokenService
    {
        /// <summary>
        /// Uygulama için JWT (JSON Web Token) kimlik doğrulama ve yetkilendirme hizmetlerini yapılandırır.
        /// Bu metod, JWT doğrulama ayarlarını yapar, token'ların nasıl işleneceğini belirtir ve yetkilendirme politikalarını ekler.
        /// </summary>
        /// <param name="services">Uygulamanın servis koleksiyonu.</param>
        /// <returns>Yapılandırılmış IServiceCollection.</returns>
        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            var key = configuration.GetSection("SysSettings:SecretKey").Value;

            var secretKey = Encoding.UTF8.GetBytes(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "testCase",
                        ValidAudience = "testCase"
                    };
                });

            return services;
        }
    }
}