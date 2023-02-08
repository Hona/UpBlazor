using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UpBlazor.Application.Exceptions;
using UpBlazor.Core.Exceptions;

namespace UpBlazor.WebApi;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<HttpResponseExceptionFilter> _logger;

    public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger) => _logger = logger;
    
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is null)
        {
            return;
        }
        
        context.Result = context.Exception switch
        {
            UpApiAccessTokenNotSetException e => new ObjectResult("Up access token is not set")
            {
                StatusCode = StatusCodes.Status403Forbidden
            },
            BadRequestException e => new ObjectResult(e.Message)
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            UpApiException e => new ObjectResult(string.Join(Environment.NewLine, e.Errors.Select(x => $"{x.Title}: {x.Detail}")))
            {
                StatusCode = (int)e.Errors.First().Status
            },
            _ => new ObjectResult("An unexpected error occurred.")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };

        if (context.Result is ObjectResult { StatusCode: 500 })
        {
            _logger.LogError(context.Exception, "An unexpected error occurred");
        }
        
        context.ExceptionHandled = true; 
    }
}