using dotnet.doduo.MessageBroker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoConsumer : IDisposable
    {

        void Listening(TimeSpan timeout, CancellationToken cancellationToken);
        Task Commit();

        event EventHandler<DoduoMessage> OnMessageReceived;

        void Reject();
    }
}
