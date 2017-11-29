using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoProducer : IDisposable
    {
        Task<ProducerResponse> ProduceAsync(string topic, byte[] body);
    }
}
