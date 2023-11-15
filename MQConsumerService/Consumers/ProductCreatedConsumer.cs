using MassTransit;
using MiniETrade.Domain.Entities.Events;
using MQConsumerService.WorkerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConsumerService.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<ProductCreated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            string message = $"ProductCreated message: {jsonMessage}";
            _logger.LogInformation(message);
            return Task.CompletedTask;
        }
    }
}