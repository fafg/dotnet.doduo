using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.MessageBroker.Model;
using dotnet.doduo.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using dotnet.doduo.Helpers;
using dotnet.doduo.DoduoQueue;

namespace dotnet.doduo.MessageBroker
{
    public class DoduoResponseHandler : IDisposable
    {
        private readonly CancellationTokenSource m_cancellationToken;
        private readonly TimeSpan m_polling = TimeSpan.FromMilliseconds(500);
        private readonly IDoduoSubscribe m_doduoSubscribe;
        private readonly IServiceProvider m_serviceProvider;
        private Task m_compositeTask;
        private List<string> _taskTopic = new List<string>();

        private readonly Guid ServerTestUid = Guid.NewGuid();

        public DoduoResponseHandler(IDoduoSubscribe doduoSubscribe, IServiceProvider serviceProvider)
        {
            m_cancellationToken = new CancellationTokenSource();
            m_doduoSubscribe = doduoSubscribe;
            m_serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            m_cancellationToken.Cancel();
            DoduoQueueResponse.Clear();
        }

        public void Start(string topic, DoduoMessageContent content)
        {
            topic = $"{topic}.response";

            DoduoQueueResponse.Create(topic);
            DoduoQueueResponse.SendRequestWait(topic, content);

            if (_taskTopic.Any(p => p.Equals(topic)))
                return;

            Task.Factory.StartNew(() =>
            {
                using (var client = m_doduoSubscribe.Build(topic, DoduoConsumerType.Response))
                {
                    RegisterMessageProcessor(client, topic);

                    client.Listening(m_polling, m_cancellationToken.Token);
                }
            }, m_cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            m_compositeTask = Task.CompletedTask;
        }

        private void RegisterMessageProcessor(IDoduoConsumer client, string topic)
        {
            client.OnResponseMessageReceived += (sender, message) =>
            {
                ProducerResponse response = null;
                try
                {
                    Console.WriteLine($"{message.Type} ---- {topic} : Server :: {ServerTestUid.ToString()}");

                    response = ProducerResponse.Ok(message.ResponseId, message);
                }
                catch (Exception e)
                {
                    response = ProducerResponse.Error(message.ResponseId, e);
                }
                DoduoQueueResponse.Send(topic, response);
                client.Commit();                
            };
        }
    }
}
