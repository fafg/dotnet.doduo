using dotnet.doduo.MessageBroker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoPublish
    {
        Task PublishAsyncWithoutReturn(string name, params object[] values);
        Task<DoduoResponse> PublishAsync(string name, params object[] values);
        Task<T> PublishAsync<T>(string name, params object[] values) where T : class;
        Task PublishResponseAsync(DoduoResponseContent response, string topic);
    }
}
