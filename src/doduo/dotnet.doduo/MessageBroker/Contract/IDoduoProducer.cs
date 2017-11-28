using dotnet.doduo.MessageBroker.Model;
using System;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoProducer : IDisposable
    {
        ProducerResponse ProduceAsync(string topic, byte[] body);
    }
}
