using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoProducer : IDisposable
    {
        Task<DoduoResponse> ProduceAsync(string topic, byte[] body);
        Task<DoduoResponse> ProduceAsync(string name, byte[] v, Guid requestId);
    }
}
