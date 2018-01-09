using dotnet.doduo.MessageBroker.Model;
using System;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoMessageBrokerConnection<T> : IDisposable where T : IDoduoProducer
    {
        IDoduoProducer Rent();
        IDoduoConsumer Consumer(string topic, DoduoConsumerType doduoConsumerType);
        bool Return(IDoduoProducer context);
    }
}
