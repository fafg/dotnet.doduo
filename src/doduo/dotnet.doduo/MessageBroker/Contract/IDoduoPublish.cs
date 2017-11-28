using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoPublish
    {
        Task PublishAsync<T>(string name, T obj) where T : class;

        Task PublishAsync(string name, IComparable value);
    }
}
