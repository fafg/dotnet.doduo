using dotnet.doduo.Configuration.Contract;
using dotnet.doduo.MessageBroker.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    internal sealed class RabbitMqBootstraper : IConfigurationExtension
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqBootstraper(RabbitMqOptions options)
        {
            _options = options;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton(_options);

            services.AddSingleton<IDoduoMessageBrokerConnection<RabbitMqDoduoProducer>, DoduoRabbitMqConnection>();
            services.AddSingleton<IDoduoProducer, RabbitMqDoduoProducer>();

            services.AddScoped<IDoduoPublish, RabbitMqPublish>();

        }
    }
}