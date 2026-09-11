using Microsoft.VisualBasic;
using RabbitMQ.Client;

namespace FleetPulse.WebAPI.Messaging
{
    public class RabbitMqConnectionManager
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);

        public RabbitMqConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is not null && _connection.IsOpen)
            {
                return _connection;
            }

            await _connectionLock.WaitAsync();

            try
            {
                if (_connection is not null && _connection.IsOpen)
                {
                    return _connection;
                }

                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"]!,
                    Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                    UserName = _configuration["RabbitMQ:UserName"]!,
                    Password = _configuration["RabbitMQ:Password"]!
                };

                _connection = await factory.CreateConnectionAsync();

                return _connection;
            }
            finally
            {
                _connectionLock.Release();
            }
        }




    }
    
}
