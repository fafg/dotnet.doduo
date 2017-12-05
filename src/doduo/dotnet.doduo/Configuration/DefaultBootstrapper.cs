using dotnet.doduo.Configuration.Contract;
using dotnet.doduo.MessageBroker;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.Configuration
{
    public class DefaultBootstrapper : IBootstrapper
    {
        private readonly IApplicationLifetime m_appLifetime;
        private readonly CancellationTokenSource m_cts;
        private readonly CancellationTokenRegistration m_ctsRegistration;
        private Task _bootstrappingTask;

        private DoduoConsumerHandler Handler { get; }

        public DefaultBootstrapper(
            IApplicationLifetime appLifetime,
            DoduoConsumerHandler handler)
        {
            m_appLifetime = appLifetime;
            Handler = handler;

            m_cts = new CancellationTokenSource();
            m_ctsRegistration = appLifetime.ApplicationStopping.Register(() =>
            {
                m_cts.Cancel();
                try
                {
                    _bootstrappingTask?.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException ex)
                {
                    throw ex;
                }
            });
        }
        public Task BootstrapAsync()
        {
            return _bootstrappingTask = BootstrapTaskAsync();
        }

        private async Task BootstrapTaskAsync()
        {
            if (m_cts.IsCancellationRequested) return;

            m_appLifetime.ApplicationStopping.Register(() =>
            {
                    Handler.Dispose();
            });

            if (m_cts.IsCancellationRequested) return;

            await BootstrapCoreAsync();

            m_ctsRegistration.Dispose();
            m_cts.Dispose();
        }

        protected virtual Task BootstrapCoreAsync()
        {
            try
            {
                Handler.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }
    }
}