using dotnet.doduo.MessageBroker.Contract;
using RabbitMQ.Client;
using System;

namespace dotnet.doduo.MessageBroker.RabbitMq
{
    public class DoduoRabbitMqConnection : IDoduoMessageBrokerConnection<RabbitMqDoduoProducer>
    {
        private readonly Func<IConnection> _createConnection;
        private IConnection _connection;
        private readonly RabbitMqOptions _options;

        private int _count;
        private int _maxSize;

        public DoduoRabbitMqConnection(RabbitMqOptions options)
        {
            _options = options;
            _createConnection = CreateConnection(options);
        }

        public IConnection GetConnection()
        {
            if (_connection != null && _connection.IsOpen)
                return _connection;
            _connection = _createConnection();
            _connection.ConnectionShutdown += ConnectionShutdown;
            return _connection;
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
            if (_connection != null && _connection.IsOpen)
                _connection.Dispose();
        }

        public IDoduoProducer Rent()
        {
            var model = GetConnection().CreateModel();
            return new RabbitMqDoduoProducer(model, _options);
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
