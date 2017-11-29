using dotnet.doduo.MessageBroker.Contract;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
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
            string json = JsonConvert.SerializeObject(obj);            
            return  _connection.Rent().ProduceAsync(name, Encoding.ASCII.GetBytes(json));            
        }

        public Task PublishAsync(string name, IComparable value)
        {
            return _connection.Rent().ProduceAsync(name, null);
        }
    }
}
