using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Exceptions;

public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problim = exception switch 
        {
            ValidationException ex => new ValidationProblemDetails(
                                            ex.Errors.GroupBy(g => g.PropertyName)
                                                    .ToDictionary(g => g.Key,
                                                                g => g.Select(e => e.ErrorMessage).ToArray()
                                                                )
                                       )
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest
            },
            NotFoundException ex => new ProblemDetails
            {
                Title = "Resource Not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            },
            _ => new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unhandled exception",
                Status = StatusCodes.Status500InternalServerError
            }
        };

        httpContext.Response.StatusCode = problim.Status!.Value;

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problim
        });

        return true;
    }
}