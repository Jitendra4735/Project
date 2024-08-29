using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;
using WebApi.Utilities;

namespace WebApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpClientException ex)
            {
                await HandleHttpClientExceptions(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private async Task HandleHttpClientExceptions(HttpContext context, HttpClientException exception)
        {
            context.Response.StatusCode = (int)exception.HttpResponseMessage.StatusCode;
            context.Response.ContentType = exception.HttpResponseMessage.Content.Headers?.ContentType.ToString();


            var error = await exception.HttpResponseMessage.Content.ReadAsStringAsync();
            await context.Response.WriteAsync(error);
        }

        private static async Task<string> GetRequestPayloadAsync(HttpContext context)
        {
            var requestMethods = new List<string> { "POST", "PUT", "PATCH" };
            string requestPayload = string.Empty;

            if (requestMethods.Exists(x => x == context.Request.Method))
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                requestPayload = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            return requestPayload;
        }
    }
}
