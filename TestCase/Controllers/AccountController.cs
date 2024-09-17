using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestCase.Models.User;
using TestCase.ResultModel;
using TestCase.Services;

namespace TestCase.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IServiceProvider _serviceProvider;

        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;
        }

        #region Login

        /// <summary>
        /// Kullanıcı adı ve parola ile giriş yapar ve geçerli bir token döndürür.
        /// Eğer model geçerli değilse, formu yeniden gösterir. Eğer kullanıcı bulunamazsa veya şifre hatalıysa uygun bir hata mesajı fırlatır.
        /// </summary>
        /// <param name="model">Giriş bilgilerini içeren <see cref="LoginModel"/> nesnesi.</param>
        /// <returns>
        /// <see cref="Ok"/> sonucu ve JWT token içeren bir JSON nesnesi döner. 
        /// Hata durumunda uygun bir HTTP hata kodu ve hata mesajı döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Kullanıcı bulunamadığında, e-posta doğrulanmadığında, şifre hatalı olduğunda veya giriş sırasında başka bir hata meydana geldiğinde fırlatılır.
        /// </exception>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                        throw new Exception("Kullanıcı bilgisi bulunamadı.");
                    else if (!await _userManager.IsEmailConfirmedAsync(user))
                        throw new Exception("E-Posta doğrulaması yapılmadı.");

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);
                    if (result.Succeeded)
                    {
                        var tokenService = new TokenService(_serviceProvider);
                        var token = tokenService.GenerateTokenAsync(user);

                        return Ok(new { Token = token });
                    }
                    else
                        throw new Exception("E-Posta yada Şifre doğru değil.");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş yaparken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}