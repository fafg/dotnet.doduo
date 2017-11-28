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
        public void Publish<T>(string name, T obj) where T : class
        {
            _connection.Rent().ProduceAsync(name, null);
            throw new NotImplementedException();
        }

        public void Publish(string name, IComparable value)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<T>(string name, T obj) where T : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string name, IComparable value)
        {
            throw new NotImplementedException();
        }

        public void PublishcOneWayAsyn(string name, IComparable value)
        {
            throw new NotImplementedException();
        }

        public void PublishOneWayAsyn<T>(string name, T obj) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
