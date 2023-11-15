using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using MiniETrade.Application.Common.Abstractions.MessageQue;
using MiniETrade.Application.Features.Products.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.MessageQue.MassTransit
{
    public class MassTransitService : IMassTransitService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;
        public MassTransitService(IPublishEndpoint publishEndpoint, IBusControl busControl, IConfiguration configuration)
        {
            _publishEndpoint = publishEndpoint;
            _busControl = busControl;
            _configuration = configuration;
        }

        public async Task Publish<T>(T message) where T : class
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task Send<T>(T message) where T : class
        {
            var sendEndPoint = await _busControl.GetSendEndpoint(new Uri(_configuration["RabbitMQ:Uri"]));
            await sendEndPoint.Send(message);
        }
    }
}