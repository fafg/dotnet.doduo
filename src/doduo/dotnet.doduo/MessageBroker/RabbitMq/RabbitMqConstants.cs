using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public static class RabbitMqConstants
    {
        public const string DEFAULT_VIRTUAL_HOST = "/";
        public const string DEFAULT_USER = "guest";
        public const string DEFAULT_PASSWORD = "guest";
        public const int DEFAULT_CONNECTION_TIMEOUT = 30 * 1000;
        public const int DEFAULT_PORT = 5672;
        public const string EXCHANGE_TYPE = "topic";
    }
}
