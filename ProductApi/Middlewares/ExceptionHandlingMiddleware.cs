using ProductApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProductApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(NotFoundException ex)
            {
                logger.LogWarning("Такого не существует");

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = 404,
                    Message = ex.Message
                };

                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
            catch(Exception ex)
            {
                logger.LogError("Случилась непредвиденная ошибка");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = 500,
                    Message = "Случилась непредвиденная ошибка. Мы уже чиним, приносим свои извинения",
                    Detailed = ex.Message
                };

                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
