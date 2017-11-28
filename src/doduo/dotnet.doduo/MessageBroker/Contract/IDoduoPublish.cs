using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoPublish
    {
        void Publish<T>(string name, T obj) where T : class;

        void Publish(string name, IComparable value);

        Task PublishAsync<T>(string name, T obj) where T : class;

        Task PublishAsync(string name, IComparable value);

        void PublishOneWayAsyn<T>(string name, T obj) where T : class;

        void PublishcOneWayAsyn(string name, IComparable value);
    }
}
