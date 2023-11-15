using MediatR;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Logging.Loggers;

public class ConsoleLogger : ILoggerService
{
    public Task LogException<TResponse>(IRequest<TResponse> request, Exception exception)
    {
        Console.WriteLine("An error occured in the request "+ request.GetType().Name);
        Debug.WriteLine("oyyy");
        Console.WriteLine("Here is the error message: "+ exception.Message);
        Console.WriteLine("Here is the stack trace: "+ exception.StackTrace);
        return Task.CompletedTask;
    }

    public Task LogMessage<TResponse>(string logMessage, IRequest<TResponse> request)
    {
        Console.WriteLine("Request Name : " + request.GetType().Name);
        Debug.WriteLine("oyyy");
        Console.WriteLine("Request details: " + JsonSerializer.Serialize(request) );
        Console.WriteLine("Log message: " + logMessage);
        return Task.CompletedTask;
    }

    public Task LogResponse<TResponse>(IRequest<TResponse> request, TResponse response)
    {
        Console.WriteLine("Request Name : " + request.GetType().Name);
        Debug.WriteLine("oyyy");
        Console.WriteLine("Request details: " + JsonSerializer.Serialize(request));
        Console.WriteLine("Response details: " + JsonSerializer.Serialize(response));
        return Task.CompletedTask;
    }

    public Task LogResponseWithMessage<TResponse>(string logMessage, IRequest<TResponse> request, TResponse response)
    {
        Console.WriteLine("Request Name : " + request.GetType().Name);
        Debug.WriteLine("oyyy");
        Console.WriteLine("Request details: " + JsonSerializer.Serialize(request));
        Console.WriteLine("Response details: " + JsonSerializer.Serialize(response));
        Console.WriteLine("Log message: " + logMessage);
        return Task.CompletedTask;
    }
}