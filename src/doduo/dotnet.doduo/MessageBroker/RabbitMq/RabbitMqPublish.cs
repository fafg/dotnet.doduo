using dotnet.doduo.MessageBroker.Contract;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqPublish : IDoduoPublish
    {
        private readonly IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> _connection;

        public RabbitMqPublish(IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> connection)
        {
            _connection = connection;
        }

        public Task PublishAsync<T>(string name, T obj) where T : class
        {
          return  _connection.Rent().ProduceAsync(name, null);            
        }

        public Task PublishAsync(string name, IComparable value)
        {
            return _connection.Rent().ProduceAsync(name, null);
        }
    }
}
