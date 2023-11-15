using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Logging
{
    public interface ILoggerFactory
    {
        ILoggerService GetLogger(LoggerType loggerType);
    }
}
