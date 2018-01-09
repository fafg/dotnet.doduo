using System.Threading.Tasks;
using dotnet.doduo.MessageBroker.Contract;
using RabbitMQ.Client;
using System.Collections.Generic;
using System;
using System.Threading;
using RabbitMQ.Client.Events;
using System.Text;
using dotnet.doduo.MessageBroker.Model;
using Newtonsoft.Json.Linq;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    internal class RabbitMqDoduoConsumer : IDoduoConsumer
    {
        private readonly string m_topic;
        private readonly DoduoConsumerType m_doduoConsumerType;
        private readonly IConnection m_connection;
        private IModel m_channel;
        private readonly RabbitMqOptions m_options;
        private ulong m_deliveryTag;

        public RabbitMqDoduoConsumer(string topic, IConnection connection, RabbitMqOptions options, DoduoConsumerType doduoConsumerType)
        {
            m_topic = topic;
            m_connection = connection;
            m_options = options;
            m_doduoConsumerType = doduoConsumerType;

            InitClient();
        }

        public event EventHandler<DoduoMessage> OnMessageReceived;
        public event EventHandler<DoduoResponseContent> OnResponseMessageReceived;

        private void InitClient()
        {
            m_channel = m_connection.CreateModel();

            m_channel.ExchangeDeclare(
                m_options.TopicExchangeName,
                RabbitMqConstants.EXCHANGE_TYPE,
                true);

            var arguments = new Dictionary<string, object> {
                { "x-message-ttl", m_options.MessageTTL }
            };

            m_channel.QueueDeclare(m_topic, true, false, false, arguments);
            m_channel.QueueBind(m_topic, m_options.TopicExchangeName, m_topic);
        }

        public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(m_channel);

            switch (m_doduoConsumerType)
            {
                case DoduoConsumerType.Request:
                    consumer.Received += OnConsumerReceived;
                    break;
                case DoduoConsumerType.Response:
                    consumer.Received += OnConsumerResponsed;
                    break;
            }

            m_channel.BasicConsume(m_topic, false, consumer);

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.WaitHandle.WaitOne(timeout);
            }
        }

        public Task Commit()
        {
            try
            {
                m_channel.BasicAck(m_deliveryTag, false);
                return Task.FromResult(ConsumerResponse.Ok());
            }
            catch (Exception ex)
            {
                return Task.FromResult(ConsumerResponse.Error(ex));
            }            
        }

        public void Reject()
        {
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            JObject content = JObject.Parse(Encoding.UTF8.GetString(e.Body));

            m_deliveryTag = e.DeliveryTag;
            var message = new DoduoMessage
            {
                Group = m_topic,
                Name = e.RoutingKey,
                Content = content.ToObject<DoduoMessageContent>()
            };
            OnMessageReceived?.Invoke(sender, message);
        }

        private void OnConsumerResponsed(object sender, BasicDeliverEventArgs e)
        {
            JObject content = JObject.Parse(Encoding.UTF8.GetString(e.Body));
            m_deliveryTag = e.DeliveryTag;

            OnResponseMessageReceived?.Invoke(sender, content.ToObject<DoduoResponseContent>());
        }

        public void Dispose()
        {
            m_channel.Dispose();
            m_connection.Dispose();
        }

        ~RabbitMqDoduoConsumer()
        {
            Dispose();
        }
    }
}