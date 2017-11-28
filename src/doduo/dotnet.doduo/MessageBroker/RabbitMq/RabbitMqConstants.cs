using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqConstants
    {
        public const string DEFAULT_VIRTUAL_HOST = "/";
        public const string DEFAULT_USER = "gest";
        public const string DEFAULT_PASSWORD = "gest";
        public const int DEFAULT_CONNECTION_TIMEOUT = 30 * 1000;
        public const int DEFAULT_PORT = 5672;
    }
}
