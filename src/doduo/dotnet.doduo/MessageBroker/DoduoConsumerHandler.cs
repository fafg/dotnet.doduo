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

namespace dotnet.doduo.MessageBroker
{
    public class DoduoConsumerHandler : IDisposable
    {
        private readonly CancellationTokenSource m_cancellationToken;
        private readonly TimeSpan m_polling = TimeSpan.FromMilliseconds(500);
        private readonly IDoduoSubscribe m_doduoSubscribe;
        private readonly DoduoConsumerSelector m_selector;
        private readonly IServiceProvider m_serviceProvider;
        private Task m_compositeTask;

        private readonly Guid ServerTestUid = Guid.NewGuid();

        public DoduoConsumerHandler(IDoduoSubscribe doduoSubscribe, DoduoConsumerSelector selector, IServiceProvider serviceProvider)
        {
            m_cancellationToken = new CancellationTokenSource();
            m_doduoSubscribe = doduoSubscribe;
            m_selector = selector;
            m_serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            var candidatesMethods = m_selector.GetCandidatesMethods();

            foreach (var candidate in candidatesMethods)
                Task.Factory.StartNew(() =>
                {
                    using (var client = m_doduoSubscribe.Build(candidate.Attribute.Topic))
                    {
                        RegisterMessageProcessor(client, candidate);

                        client.Listening(m_polling, m_cancellationToken.Token);
                    }
                }, m_cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            m_compositeTask = Task.CompletedTask;
        }

        private void RegisterMessageProcessor(IDoduoConsumer client, DoduoConsumerExecutor candidate)
        {
            client.OnMessageReceived += (sender, message) =>
            {
                try
                {
                    Console.WriteLine($"{message.Content}   ---- {candidate.Attribute.Topic} : {candidate.MethodInfo.Name}, {candidate.ImplTypeInfo.Name} : Server :: {ServerTestUid.ToString()}");

                    InvokeAsync(candidate, message).Wait();
                    client.Commit();
                }
                catch (Exception e)
                {
                    client.Reject();
                }
            };
        }

        private async Task InvokeAsync(DoduoConsumerExecutor candidate, DoduoMessage doduoMessage)
        {
            var executor = ObjectMethodExecutor.Create(
               candidate.MethodInfo,
                candidate.ImplTypeInfo);

            using (var scope = m_serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var serviceType = candidate.ImplTypeInfo.AsType();
                var obj = ActivatorUtilities.GetServiceOrCreateInstance(provider, serviceType);

                var jsonContent = doduoMessage.Content;

                object resultObj;
                if (executor.MethodParameters.Length > 0)
                    resultObj = await ExecuteWithParameterAsync(executor, obj, jsonContent);
            }
        }

        private async Task<object> ExecuteWithParameterAsync(ObjectMethodExecutor executor, object controller, DoduoMessageContent content)
        {
            try
            {
                IEnumerable<object> messageParameters = ParameterBuilderHelper.BuildParameters(executor.MethodParameters, content);
                int ssss = messageParameters.Count();
                if (executor.IsMethodAsync)
                    return await executor.ExecuteAsync(controller, messageParameters.ToArray());
                return executor.Execute(controller, messageParameters.ToArray());

                throw new Exception($"Parameters: `xpto` bind failed! ParameterString is:  ");
            }
            catch (FormatException ex)
            {
                return null;
            }
        }

        private IEnumerable<object> GetBindObjects(IEnumerable<ParameterInfo> parameters, DoduoMessageContentObject[] objects)
        {
            foreach (var parameter in parameters)
            {
                DoduoMessageContentObject obj = objects.Where(p => p.PropertyName == parameter.ParameterType.ToString()).FirstOrDefault();
                if (obj != null)
                    yield return JObject.Parse(obj.Value.ToString()).ToObject(parameter.ParameterType);
            }
        }
    }
}
