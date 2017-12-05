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
        private readonly IConnection m_connection;
        private IModel m_channel;
        private readonly RabbitMqOptions m_options;
        private ulong m_deliveryTag;

        public RabbitMqDoduoConsumer(string topic, IConnection connection, RabbitMqOptions options)
        {
            m_topic = topic;
            m_connection = connection;
            m_options = options;

            InitClient();
        }

        public event EventHandler<DoduoMessage> OnMessageReceived;

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
            consumer.Received += OnConsumerReceived;

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
            m_deliveryTag = e.DeliveryTag;
            var message = new DoduoMessage
            {
                Group = m_topic,
                Name = e.RoutingKey,
                Content = JObject.Parse(Encoding.UTF8.GetString(e.Body)).ToObject<DoduoMessageContent>()
            };
            OnMessageReceived?.Invoke(sender, message);
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