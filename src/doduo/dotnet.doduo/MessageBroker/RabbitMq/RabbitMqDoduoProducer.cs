using dotnet.doduo.MessageBroker.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using dotnet.doduo.MessageBroker.Model;
using RabbitMQ.Client;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class RabbitMqDoduoProducer : IDoduoProducer
    {
        private readonly IModel _model;
        private readonly RabbitMqOptions _options;
        public RabbitMqDoduoProducer(IModel model, RabbitMqOptions options)
        {
            _model = model;
            _options = options;
        }

        public ProducerResponse ProduceAsync(string topic, byte[] body)
        {
            try
            {
                _model.ExchangeDeclare(_options.TopicExchangeName, RabbitMqConstants.EXCHANGE_TYPE, true);
                _model.BasicPublish(_options.TopicExchangeName,
                        topic,
                        null,
                        body);

                return ProducerResponse.Ok();
            }
            catch (Exception ex)
            {
                return ProducerResponse.Error(ex);
            }

        }

        public void Dispose()
        {
            _model.Dispose();
        }

        ~RabbitMqDoduoProducer()
        {
            Dispose();
        }
    }
}
