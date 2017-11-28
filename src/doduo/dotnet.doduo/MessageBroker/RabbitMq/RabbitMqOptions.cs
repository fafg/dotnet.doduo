using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }
        public string User { get; set; } = RabbitMqConstants.DEFAULT_USER;
        public string Password { get; set; } = RabbitMqConstants.DEFAULT_PASSWORD;
        public int Port { get; set; } = RabbitMqConstants.DEFAULT_PORT;
        public string VirtualHost { get; set; } = RabbitMqConstants.DEFAULT_VIRTUAL_HOST;
        public int ConnectionTimeout { get; set; } = RabbitMqConstants.DEFAULT_CONNECTION_TIMEOUT;
        public int ReadTimeout { get; set; } = RabbitMqConstants.DEFAULT_CONNECTION_TIMEOUT;
        public int WriteTimeout { get; set; } = RabbitMqConstants.DEFAULT_CONNECTION_TIMEOUT;
        public string TopicExchangeName { get; set; }

        public static RabbitMqOptions Default()
        {
            return _instanceDefaultStatic;
        }

        private readonly static RabbitMqOptions _instanceDefaultStatic = new RabbitMqOptions
        {
            Host = "localhost",
            TopicExchangeName = "doduo.default"

        };

    }
}
