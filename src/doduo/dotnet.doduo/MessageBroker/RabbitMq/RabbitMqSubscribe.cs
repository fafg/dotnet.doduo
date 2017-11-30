using dotnet.doduo.MessageBroker.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using dotnet.doduo.MessageBroker.Model;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqSubscribe : IDoduoSubscribe
    {
        private readonly IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> m_connection;

        public RabbitMqSubscribe(IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> connection)
        {
            m_connection = connection;
        }

        public IDoduoConsumer Build(string topic)
        {
            return m_connection.Consumer(topic);
        }
    }
}
