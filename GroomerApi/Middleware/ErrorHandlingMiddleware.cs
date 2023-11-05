using GroomerApi.Exceptions;

namespace GroomerApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context); //Czyli podczas uzywania programu jeśli wyskkoczy błąd który sami dodaliśmy np. NotFaoundException to wpadnie w Catch NotFoundException, reszta błędów'Exception' wpadnie do catch nr 2.
            }
            catch(NotFoundException notfoundExceptiond)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notfoundExceptiond.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
