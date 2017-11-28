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
           return configuration.UseRabbitMQ(opt => opt = RabbitMqOptions.Default());
        }
        public static DoduoConfiguration UseRabbitMQ(this DoduoConfiguration configuration, Action<RabbitMqOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            configuration.RegisterExtension(new RabbitMqBootstraper(configure));

            return configuration;
        }
    }
}