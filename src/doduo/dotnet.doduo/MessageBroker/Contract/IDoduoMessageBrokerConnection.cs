﻿using System;

namespace dotnet.doduo.MessageBroker.Contract
{
    public interface IDoduoMessageBrokerConnection<T> : IDisposable where T : IDoduoProducer
    {
        IDoduoProducer Rent();
        IDoduoConsumer Consumer(string topic);
        bool Return(IDoduoProducer context);
    }
}
