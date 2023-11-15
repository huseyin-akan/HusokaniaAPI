using MassTransit;
using MQConsumerService.Consumers;
using MQConsumerService.WorkerServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<ProductCreatedConsumer>();

            configurator.UsingRabbitMq ( (context, _configurator) =>
            {
                _configurator.Host(hostContext.Configuration["RabbitMQ:Uri"], h =>
                {
                    h.Username(hostContext.Configuration["RabbitMQ:Username"]);
                    h.Password(hostContext.Configuration["RabbitMQ:Password"]);
                });

                _configurator.ReceiveEndpoint("product-created-event", e => e.ConfigureConsumer<ProductCreatedConsumer>(context));
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
