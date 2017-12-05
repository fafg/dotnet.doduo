using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoSubscribe
    {
        IDoduoConsumer Build(string topic);
    }
}
