using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoPublish
    {
        Task PublishAsync(string name, params object[] values);
    }
}
