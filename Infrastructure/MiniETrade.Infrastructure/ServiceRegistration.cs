using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Logging;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Application.Common.Abstractions.Storage;
using MiniETrade.Infrastructure.Enums;
using MiniETrade.Infrastructure.Services;
using MiniETrade.Infrastructure.Services.Caching;
using MiniETrade.Infrastructure.Services.Identity;
using MiniETrade.Infrastructure.Services.Logging;
using MiniETrade.Infrastructure.Services.Logging.Loggers;
using MiniETrade.Infrastructure.Services.Security;
using MiniETrade.Infrastructure.Services.Storage;
using MiniETrade.Infrastructure.Services.Storage.Local;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IStorageService, StorageService>();
        serviceCollection.AddScoped<ITokenHelper, TokenHelper>();
        serviceCollection.AddSingleton<JwtSecurityTokenHandler>();
        //serviceCollection.AddScoped<IMQPublisherService, RabbitMQService>(); TODO-HUS bu servisler şimdilik kapatıldı. Şirket networkünden clouda bağlanamıyorum.
        //serviceCollection.AddScoped<IMQConsumerService, RabbitMQService>();
        //serviceCollection.AddScoped<IMassTransitService, MassTransitService>(); 

        serviceCollection.AddScoped<ICachingService, DistributedCachingService>();
        serviceCollection.AddScoped<IIdentityService, IdentityService>();

        serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
        //serviceCollection.AddSingleton<ILanguageService, LanguageService>();

        //Redis Caching
        //serviceCollection.AddStackExchangeRedisCache( options => {
        //    options.Configuration = configuration["Redis:Uri"];
        //    });            
        //.Net In-memory distributed caching
        serviceCollection.AddDistributedMemoryCache(); //Inmemory distributed cache implementasyonu olarak kullanmak istersen.

        //Logging
        serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
        serviceCollection.AddScoped<FileLogger>();
        serviceCollection.AddScoped<ConsoleLogger>();
        serviceCollection.AddScoped<EFLogger>();
        serviceCollection.AddScoped<MongoLogger>();
        serviceCollection.AddScoped<ILoggerService, EFLogger>();

        //Stroge Management Registration
        serviceCollection.AddStorage(StorageType.Local);

        //App-settings options
        serviceCollection.Configure<DistributedCachingOptions>(configuration.GetSection("DistributedCachingOptions"));
    }

    //MassTransit Configuration
    public static void AddMassTransitRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMassTransit(
            config =>
            {
                config.UsingRabbitMq((context, _config) =>
                {
                    _config.Host(configuration["RabbitMQ:Uri"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });
                });
            });
    }

    public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
    {
        serviceCollection.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
    {
        switch (storageType)
        {
            case StorageType.Local:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
            case StorageType.Azure:
                //serviceCollection.AddScoped<IStorage, AzureStorage>(); //TODO-HUS
                break;
            case StorageType.AWS:

                break;
            default:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
        }
    }
}