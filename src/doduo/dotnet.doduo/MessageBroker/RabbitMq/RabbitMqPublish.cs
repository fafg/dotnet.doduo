using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.MessageBroker.Model;
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
        private readonly IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> m_connection;

        public RabbitMqPublish(IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> connection)
        {
            m_connection = connection;
        }

        public Task PublishAsync(string name, params object[] values)
        {
            List<DoduoMessageContentObject> objects = new List<DoduoMessageContentObject>();
            foreach (var value in values)
            {
                if (value.GetType().IsPrimitive || value is IConvertible)
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = value, IsPrimitive = true });
                else
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = JsonConvert.SerializeObject(value) });
            }

            DoduoMessageContent contnet = new DoduoMessageContent();
            contnet.Objects = objects.ToArray();

            string json = JsonConvert.SerializeObject(contnet);
            using (var response = m_connection.Rent().ProduceAsync(name, Encoding.ASCII.GetBytes(json)))
                return response;
        }
    }
}
