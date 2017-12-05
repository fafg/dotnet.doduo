using dotnet.doduo.MessageBroker.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using dotnet.doduo.MessageBroker.Model;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqDoduoProducer : IDoduoProducer
    {
        private readonly IModel m_model;
        private readonly RabbitMqOptions m_options;
        public RabbitMqDoduoProducer(IModel model, RabbitMqOptions options)
        {
            m_model = model;
            m_options = options;
        }

        public Task<ProducerResponse> ProduceAsync(string topic, byte[] body)
        {
            try
            {
                m_model.ExchangeDeclare(m_options.TopicExchangeName, RabbitMqConstants.EXCHANGE_TYPE, true);

                m_model.BasicPublish(m_options.TopicExchangeName,
                        topic,
                        null,
                        body);

                return Task.FromResult(ProducerResponse.Ok());
            }
            catch (Exception ex)
            {
                return Task.FromResult(ProducerResponse.Error(ex));
            }
        }

        public void Dispose()
        {
            m_model.Dispose();
        }

        ~RabbitMqDoduoProducer()
        {
            Dispose();
        }
    }
}
