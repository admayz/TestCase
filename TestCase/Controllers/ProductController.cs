using AutoMapper;
using FluentValidation;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestCase.Models;
using TestCase.Repositories;
using TestCase.ResultModel;
using TestCase.ViewModel;

namespace TestCase.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private IValidator<Product> _validator;
        private readonly ProductRepository repoProduct;

        public ProductController(ILogger<ProductController> logger, IServiceProvider serviceProvider, IMapper mapper, IValidator<Product> validator, AppData context)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _validator = validator;
            repoProduct = new ProductRepository(context);
        }

        #region Get

        /// <summary>
        /// Verilen ID'ye sahip ürünü getirir.
        /// </summary>
        /// <param name="model">Ürünün ID'sini içeren <see cref="IdModel"/> nesnesi.</param>
        /// <returns>
        /// Ürün bilgilerini içeren bir <see cref="ProductViewModel"/> döner. 
        /// Hata durumunda uygun bir HTTP hata kodu ve hata mesajı döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Model boş olduğunda veya ürün bulunamadığında fırlatılır.
        /// </exception>
        [HttpGet]
        public async Task<IActionResult> GetById(IdModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Model boş bırakılamaz.");

                var item = await repoProduct.GetItemAsync(model.Id);
                if (item == null)
                    throw new Exception("Ürün bulunamadı.");

                var resultModel = _mapper.Map<ProductViewModel>(item);
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün getirilirken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>
        /// Tüm ürünleri listeler.
        /// </summary>
        /// <returns>
        /// Ürünlerin listesini içeren bir <see cref="List{ProductViewModel}"/> döner. 
        /// Hata durumunda uygun bir HTTP hata kodu ve hata mesajı döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Ürün bulunamadığında fırlatılır.
        /// </exception>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var items = repoProduct.GetList();
                if (items.IsNullOrEmpty())
                    throw new Exception("Herhangi bir ürün bulunamadı.");

                var resultModel = _mapper.Map<List<ProductViewModel>>(items);
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler getirilirken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Create

        /// <summary>
        /// Yeni bir ürün ekler.
        /// </summary>
        /// <param name="model">Eklemek için gerekli ürün bilgilerini içeren <see cref="ProductViewModel"/> nesnesi.</param>
        /// <returns>
        /// Başarı durumunda boş bir <see cref="NoContent"/> döner. 
        /// Doğrulama hataları varsa, model hatalarını içeren bir <see cref="BadRequest"/> döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Model boş olduğunda veya ürün eklenirken hata meydana geldiğinde fırlatılır.
        /// </exception>
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Model boş bırakılamaz.");

                Product product = _mapper.Map<Product>(model);
                FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(product);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }

                _ = repoProduct.CreateAsync(product);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün kayıt edilirken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Var olan bir ürünü günceller. (Yalnızca Admin için geçerlidir)
        /// </summary>
        /// <param name="model">Güncellenmesi gereken ürün bilgilerini içeren <see cref="ProductViewModel"/> nesnesi.</param>
        /// <returns>
        /// Başarı durumunda boş bir <see cref="NoContent"/> döner. 
        /// Doğrulama hataları varsa, model hatalarını içeren bir <see cref="BadRequest"/> döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Model boş olduğunda, ürün bulunamadığında veya güncellenirken hata meydana geldiğinde fırlatılır.
        /// </exception>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Model boş bırakılamaz.");

                var product = await repoProduct.GetItemAsync((Guid)model.Id);
                if (product == null)
                    throw new Exception("Ürün bulunamadı.");

                _mapper.Map(model, product);
                FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(product);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }

                await repoProduct.EditAsync(product);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncellenirken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Var olan bir ürünü siler. (Yalnızca Admin için geçerlidir)
        /// </summary>
        /// <param name="model">Silinmesi gereken ürünün ID'sini içeren <see cref="IdModel"/> nesnesi.</param>
        /// <returns>
        /// Başarı durumunda boş bir <see cref="NoContent"/> döner. 
        /// Hata durumunda uygun bir HTTP hata kodu ve hata mesajı döner.
        /// </returns>
        /// <exception cref="Exception">
        /// Model boş olduğunda, ürün bulunamadığında veya silinirken hata meydana geldiğinde fırlatılır.
        /// </exception>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(IdModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Model boş bırakılamaz.");

                var item = await repoProduct.GetItemAsync(model.Id);
                if (item == null)
                    throw new Exception("Ürün bulunamadı.");

                await repoProduct.DeleteAsync(item);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata meydana geldi.");
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}