using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.BusinessRules;
using MiniETrade.Application.Common.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(ServiceRegistration));

            //Böylece aşağıdaki kodda demiş olduk ki, BaseBusinessRuless'un çocuğu olan tüm tipleri IoC'ye ekle.
            serviceCollection.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            //TODO-HUS bu performans arkadaşı her istekte çalıştığı takdirde sorun oluyor. Bunu her istekte çalıştırcaksak UserId ile beraber çalıştırmamalıyız. Ya da UserId null kontrolü yapılmalı sanırım.
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
        }

        //Assembly'de yani bu projede verilen tipi arayıp, buluyor ve onun bir instance'ını IoC'ye ekliyor.
        public static IServiceCollection AddSubClassesOfType(this IServiceCollection services,
             Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle is null) services.AddScoped(item);
                else addWithLifeCycle(services, type);
            return services;
        }
    }
}
