using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Logging
{
    public interface ILoggerService
    {
        Task LogException<TResponse>(IRequest<TResponse> request, Exception exception);
        Task LogResponse<TResponse>(IRequest<TResponse> request, TResponse response);
        Task LogResponseWithMessage<TResponse>(string logMessage, IRequest<TResponse> request, TResponse response);
        Task LogMessage<TResponse>(string logMessage, IRequest<TResponse> request);
    }
}