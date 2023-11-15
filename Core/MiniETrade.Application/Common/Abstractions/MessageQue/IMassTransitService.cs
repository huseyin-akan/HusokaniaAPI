using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.MessageQue
{
    public interface IMassTransitService
    {
        Task Publish<T>(T message) where T : class;
        Task Send<T>(T message) where T : class;
    }
}
