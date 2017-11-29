using dotnet.doduo.MessageBroker.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo
{
    public static class RabbitMqOptionsExtensions
    {
        public static DoduoConfiguration UseRabbitMQ(this DoduoConfiguration configuration)
        {
           return configuration.UseRabbitMQ( RabbitMqOptions.Default());
        }
        public static DoduoConfiguration UseRabbitMQ(this DoduoConfiguration configuration, Action<RabbitMqOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var options = new RabbitMqOptions();
            configure?.Invoke(options);

            return configuration.UseRabbitMQ(options);
        }

        public static DoduoConfiguration UseRabbitMQ(this DoduoConfiguration configuration,RabbitMqOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            configuration.RegisterExtension(new RabbitMqBootstraper(options));

            return configuration;
        }
    }
}