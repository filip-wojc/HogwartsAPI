using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.WebSockets;
using System.Security.Claims;

namespace HogwartsAPI.Exceptions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;
        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string message) = exception switch
            {
                ForbidException ex => (403, ex.Message),
                NotFoundException ex => (404, ex.Message),
                BadHttpRequestException ex => (400, ex.Message),
                _ => default
            };

            if (statusCode == default)
            {
                return false;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(message);

            _logger.LogError(statusCode, message, exception.Message);
            return true;
        }
    }
}
