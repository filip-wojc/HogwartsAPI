using Microsoft.AspNetCore.Diagnostics;

namespace HogwartsAPI.Exceptions
{
    public class GeneralExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GeneralExceptionHandler> _logger;
        public GeneralExceptionHandler(ILogger<GeneralExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync(exception.Message);
            _logger.LogError(exception.Message, exception);
            return true;
        }
    }
}
