using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.MessageBroker.Model;
using dotnet.doduo.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly DoduoResponseHandler m_doduoResponseHandler;
        private readonly DoduoApplicationIdentifier m_applicationIdentifier;

        public RabbitMqPublish(IDoduoMessageBrokerConnection<RabbitMqDoduoProducer> connection,
            DoduoResponseHandler doduoResponseHandler, DoduoApplicationIdentifier applicationIdentifier)
        {
            m_connection = connection;
            m_doduoResponseHandler = doduoResponseHandler;
            m_applicationIdentifier = applicationIdentifier;
        }

        public Task PublishAsyncWithoutReturn(string name, params object[] values)
        {
            string json = JsonConvert.SerializeObject(GetProducer(values));

            using (var response = m_connection.Rent().ProduceAsync(name, Encoding.ASCII.GetBytes(json)))
                return response;
        }

        public Task PublishResponseAsync(DoduoResponseContent doduoResponse, string topic)
        {
            string json = JsonConvert.SerializeObject(doduoResponse);
            
            using (var response = m_connection.Rent().ProduceAsync($"{topic}.response.{m_applicationIdentifier.ApplicationId}", Encoding.ASCII.GetBytes(json)))
                return response;
        }

        private object GetProducer(object requestId, object values)
        {
            throw new NotImplementedException();
        }

        public async Task<DoduoResponse> PublishAsync(string name, params object[] values)
        {
            DoduoMessageContent content = GetProducer(values);
            string json = JsonConvert.SerializeObject(content);
            m_doduoResponseHandler.Start(name, content);

            using (var response = m_connection.Rent().ProduceAsync(name, Encoding.ASCII.GetBytes(json), content.RequestId))
            {
                return await response.ConfigureAwait(false);
            }                
        }

        private DoduoMessageContent GetProducer(params object[] values)
        {
            List<DoduoMessageContentObject> objects = new List<DoduoMessageContentObject>();
            foreach (var value in values)
            {
                if (value.GetType().IsPrimitive || value is IConvertible)
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = value, IsPrimitive = true });
                else
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = JsonConvert.SerializeObject(value) });
            }

            DoduoMessageContent content = new DoduoMessageContent();
            content.Objects = objects.ToArray();

            return content;
        }

        private DoduoMessageContent GetProducer(Guid requestId, params object[] values)
        {
            List<DoduoMessageContentObject> objects = new List<DoduoMessageContentObject>();
            foreach (var value in values)
            {
                if (value.GetType().IsPrimitive || value is IConvertible)
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = value, IsPrimitive = true });
                else
                    objects.Add(new DoduoMessageContentObject() { PropertyName = value.GetType().ToString(), Value = JsonConvert.SerializeObject(value) });
            }

            DoduoMessageContent content = new DoduoMessageContent(requestId);
            content.Objects = objects.ToArray();

            return content;
        }

        public async Task<T> PublishAsync<T>(string name, params object[] values) where T : class
        {
            var response = await PublishAsync(name, values);
            return JObject.Parse(response.Body.Body).ToObject<T>();
        }
    }
}
