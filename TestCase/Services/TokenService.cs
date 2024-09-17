using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestCase.Models.User;
using TestCase.Utlis;

namespace TestCase.Services
{
    public class TokenService
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Verilen kullanıcı için JWT oluşturur.
        /// </summary>
        /// <param name="user">JWT oluşturulacak kullanıcı.</param>
        /// <returns>Oluşturulan JWT.</returns>
        public async Task<string> GenerateTokenAsync(AppUser user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
               var _userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<AppUser>>();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Consts.SysSettings.SecretKey);

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                });

                foreach (var role in roles)
                {
                    claims.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "testCase",
                    Audience = "testCase",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
}
