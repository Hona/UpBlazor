using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UpBlazor.Application;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var startTime = DateTime.Now;
        
        // Request
        var myType = request.GetType();
        var props = new List<PropertyInfo>(myType.GetProperties());

        var valueDictionary = new Dictionary<string, object>();
        
        foreach (var prop in props)
        {
            var propValue = prop.GetValue(request, null);
            valueDictionary[prop.Name] = propValue;
        }
        
        _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);
        _logger.LogDebug("Handling {Request} ({@Value})", typeof(TRequest).Name, valueDictionary);

        var response = await next();
        
        // Response
        _logger.LogInformation("Handled {Request} {Response} in {Delta}ms", typeof(TRequest).Name, typeof(TResponse).Name, (DateTime.Now - startTime).TotalMilliseconds.ToString("F2"));
        
        return response;
    }
}