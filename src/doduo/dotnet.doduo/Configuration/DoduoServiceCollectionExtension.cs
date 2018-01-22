using dotnet.doduo;
using dotnet.doduo.Configuration;
using dotnet.doduo.Configuration.Contract;
using dotnet.doduo.MessageBroker;
using dotnet.doduo.Model;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static DoduoBuilder AddDoduo(this IServiceCollection services, Action<DoduoConfiguration> setupAction)
        {
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            services.Configure(setupAction);
            AddSubscribeServices(services);

            services.AddSingleton<DoduoApplicationIdentifier>();
            services.AddSingleton<DoduoConsumerSelector>();

            services.AddSingleton<DoduoConsumerHandler>();
            services.AddSingleton<DoduoResponseHandler>(); 
            services.AddSingleton<IBootstrapper, DefaultBootstrapper>();

            DoduoConfiguration options = new DoduoConfiguration();
            setupAction.Invoke(options);

            foreach (var serviceExtension in options.Extensions)
                serviceExtension.AddServices(services);
            services.AddSingleton(options);

            return new DoduoBuilder(services);
        }

        private static void AddSubscribeServices(IServiceCollection services)
        {
            var consumerListenerServices = new List<KeyValuePair<Type, Type>>();

            foreach (var service in consumerListenerServices)
                services.AddTransient(service.Key, service.Value);
        }

    }
}
