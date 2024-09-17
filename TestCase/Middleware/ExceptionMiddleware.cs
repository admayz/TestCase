using Newtonsoft.Json;
using System.Net;
using TestCase.ResultModel;

namespace TestCase.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// İstekleri işlemek için middleware'i kullanır ve bir istisna durumunda istisnayı yakalar.
        /// </summary>
        /// <param name="context">HTTP isteği ve yanıtını temsil eden <see cref="HttpContext"/> nesnesi.</param>
        /// <returns>Asenkron bir görev döner.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middlware'da hata yakalandı.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// İstisna durumunda HTTP yanıtını oluşturur ve istisna bilgilerini yanıt olarak gönderir.
        /// </summary>
        /// <param name="context">HTTP isteği ve yanıtını temsil eden <see cref="HttpContext"/> nesnesi.</param>
        /// <param name="ex">Yakalanan <see cref="Exception"/> nesnesi.</param>
        /// <returns>Asenkron bir görev döner.</returns>
        public static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var responseObject = new ResponseModel
            {
                Success = false,
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            var jsonResponse = JsonConvert.SerializeObject(responseObject);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
