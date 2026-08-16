using Microsoft.AspNetCore.Diagnostics;

namespace ECommerce.API.Middlewares;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception , "Unhandled Exception Occured");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = "Internal Server Error",
                Title = " Unexpected Error"
            }
        };
        return await problemDetailsService.TryWriteAsync(problemDetails);
    }
}
