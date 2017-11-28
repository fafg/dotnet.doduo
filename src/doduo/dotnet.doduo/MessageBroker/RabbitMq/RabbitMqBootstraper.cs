using dotnet.doduo.Configuration.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    internal sealed class RabbitMqBootstraper : IConfigurationExtension
    {
        private readonly Action<RabbitMqOptions> _configure;

        public RabbitMqBootstraper(Action<RabbitMqOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new RabbitMqOptions();
            _configure?.Invoke(options);
            services.AddSingleton(options);

        }
    }
}