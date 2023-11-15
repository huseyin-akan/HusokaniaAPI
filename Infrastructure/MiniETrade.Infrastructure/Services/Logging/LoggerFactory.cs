using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.Common.Abstractions.Logging;
using MiniETrade.Infrastructure.Services.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LoggerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILoggerService GetLogger(LoggerType loggerType)
        {
            ILoggerService? logger = loggerType switch
            {
                LoggerType.FileLogger => _serviceProvider.GetService<FileLogger>(),
                LoggerType.ConsoleLogger => _serviceProvider.GetService<ConsoleLogger>(),
                LoggerType.EFLogger => _serviceProvider.GetService<EFLogger>(),
                LoggerType.MongoLogger => _serviceProvider.GetService<MongoLogger>(),
                _ => throw new ArgumentOutOfRangeException(nameof(loggerType), "Invalid logger type."),
            };

            if (logger is null) throw new Exception("Logger service cannot be null");
            return logger;
        }
    }
}
