using MediatR;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Logging.Loggers
{
    public class MongoLogger : ILoggerService
    {
        public Task LogException<TResponse>(IRequest<TResponse> request, Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task LogMessage<TResponse>(string logMessage, IRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public Task LogResponse<TResponse>(IRequest<TResponse> request, TResponse response)
        {
            throw new NotImplementedException();
        }

        public Task LogResponseWithMessage<TResponse>(string logMessage, IRequest<TResponse> request, TResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
