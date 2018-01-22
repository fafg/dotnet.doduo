using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.MessageBroker.Model;
using dotnet.doduo.Model;
using dotnet.doduo.Model.Tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker
{
    public class DoduoResponseHandler : IDisposable
    {
        private readonly CancellationTokenSource m_cancellationToken;
        private readonly TimeSpan m_polling = TimeSpan.FromMilliseconds(500);
        private readonly IDoduoSubscribe m_doduoSubscribe;
        private readonly IServiceProvider m_serviceProvider;
        private readonly DoduoApplicationIdentifier m_applicationIdentifier;
        private Task m_compositeTask;
        private List<string> m_taskTopic = new List<string>();

        private readonly Guid ServerTestUid = Guid.NewGuid();

        public DoduoResponseHandler(IDoduoSubscribe doduoSubscribe, IServiceProvider serviceProvider, DoduoApplicationIdentifier applicationIdentifier)
        {
            m_cancellationToken = new CancellationTokenSource();
            m_doduoSubscribe = doduoSubscribe;
            m_serviceProvider = serviceProvider;
            m_applicationIdentifier = applicationIdentifier;
        }

        public void Dispose()
        {
            m_cancellationToken.Cancel();
            DoduoTierSingleton.Instance.Clear();
        }

        public void Start(string topic, DoduoMessageContent content)
        {
            topic = $"{topic}.response.{m_applicationIdentifier.ApplicationId}";
            DoduoTierSingleton.Instance.SendRequestWait(topic, content);

            if (m_taskTopic.Any(p => p.Equals(topic)))
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

                DoduoTierSingleton.Instance.Send(topic, response).ConfigureAwait(false);
                client.Commit();                
            };
        }
    }
}
