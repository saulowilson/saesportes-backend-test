using System.Text;
using System.Text.Json;
using api.Config;
using api.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace api.Providers
{
    public class RabbitMQConnection
    {
        private readonly ILogger<RabbitMQConnection> _logger;
        private readonly AppSettings _appSettings;

        public RabbitMQConnection(
            ILogger<RabbitMQConnection> logger,
            IOptions<AppSettings> appSettings
        )
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public string PostMessage(MessagePayload data)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _appSettings.RabbitMQHost ?? "localhost",
                    UserName = _appSettings.RabbitMQUser ?? "user",
                    Password = _appSettings.RabbitMQPassword ?? "password",
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "message",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var json = JsonSerializer.Serialize(data);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: "message",
                    basicProperties: null,
                    body: body
                );
                Console.WriteLine($" [x] Sent {json}");
                return $"{json} sent";
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
