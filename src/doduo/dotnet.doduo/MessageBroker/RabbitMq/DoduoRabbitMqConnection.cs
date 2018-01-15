﻿using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.MessageBroker.Model;
using dotnet.doduo.Model;
using RabbitMQ.Client;
using System;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class DoduoRabbitMqConnection : IDoduoMessageBrokerConnection<RabbitMqDoduoProducer>
    {
        private readonly Func<IConnection> m_createConnection;
        private IConnection m_connection;
        private readonly RabbitMqOptions m_options;
        private readonly DoduoApplicationIdentifier m_applicationIdentifier;

        private int m_count;
        private int m_maxSize;

        public DoduoRabbitMqConnection(RabbitMqOptions options, DoduoApplicationIdentifier applicationIdentifier)
        {
            m_options = options;
            m_applicationIdentifier = applicationIdentifier;
            m_createConnection = CreateConnection(options);
        }

        public IConnection GetConnection()
        {
            if (m_connection != null && m_connection.IsOpen)
                return m_connection;
            m_connection = m_createConnection();
            m_connection.ConnectionShutdown += ConnectionShutdown;
            return m_connection;
        }

        private void ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //TODO : Create the log
            Console.WriteLine($"RabbitMQ client closed!");
        }
        private static Func<IConnection> CreateConnection(RabbitMqOptions options)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Host,
                UserName = options.User,
                Port = options.Port,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                RequestedConnectionTimeout = options.ConnectionTimeout,
                SocketReadTimeout = options.ReadTimeout,
                SocketWriteTimeout = options.WriteTimeout
            };

            return () => factory.CreateConnection();
        }

        public void Dispose()
        {
            if (m_connection != null && m_connection.IsOpen)
                m_connection.Dispose();
        }

        public IDoduoProducer Rent()
        {       
            var model = GetConnection().CreateModel();
            return new RabbitMqDoduoProducer(model, m_options, m_applicationIdentifier);
        }

        public IDoduoConsumer Consumer(string topic, DoduoConsumerType doduoConsumerType)
        {
            return new RabbitMqDoduoConsumer(topic, GetConnection(), m_options, doduoConsumerType, m_applicationIdentifier);
        }  
        

        public bool Return(IDoduoProducer context)
        {
            throw new NotImplementedException();
        }

        ~DoduoRabbitMqConnection()
        {
            Dispose();
        }
    }
}
