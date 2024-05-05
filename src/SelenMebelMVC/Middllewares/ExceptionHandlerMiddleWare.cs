using Newtonsoft.Json;
using SelenMebel.Service.Exceptions;
using SelenMebelMVC.Helpers;
using SelenMebelMVC.Models;

namespace SelenMebelMVC.Middllewares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (StatusCodeException exception)
            {
                await UserErrorHandlerAsync(exception, context);
            }
            catch (SelenMebelException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new Response
                {
                    Code = ex.StatusCode,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}\n\n");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new Response
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        public async Task UserErrorHandlerAsync(StatusCodeException exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            ErrorDto dto = new ErrorDto()
            {
                StatusCode = (int)exception.StatusCode,
                Message = exception.Message
            };
            string jsonData = JsonConvert.SerializeObject(dto);
            context.Response.StatusCode = (int)exception.StatusCode;
            await context.Response.WriteAsync(jsonData);
        }
        public async Task ServiceErrorHandlerAsync(Exception exception, HttpContext context)
        {
            ErrorDto dto = new()
            {
                Message = exception.Message,
                StatusCode = 500
            };
            string jsonData = JsonConvert.SerializeObject(dto);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(jsonData);
        }
    }
}
