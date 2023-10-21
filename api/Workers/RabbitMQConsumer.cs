using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using api.Hubs;
using api.Config;
using Microsoft.Extensions.Options;

namespace api.Workers
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IHubContext<MessageHub> _hub;
        private readonly AppSettings _appSettings;

        public RabbitMQConsumer(
            ILogger<RabbitMQConsumer> logger,
            IHubContext<MessageHub> hub,
            IOptions<AppSettings> appSettings
        )
        {
            _logger = logger;
            _hub = hub;
            _appSettings = appSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(_appSettings.RabbitMQHost);
            _logger.LogInformation(_appSettings.RabbitMQUser);
            _logger.LogInformation(_appSettings.RabbitMQPassword);
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: "message", autoAck: true, consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    //logger.LogInformation($"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            _logger.LogInformation(
                $"[Nova mensagem | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] "
                    + Encoding.UTF8.GetString(e.Body.ToArray())
            );
            _hub.Clients.All.SendAsync("message", Encoding.UTF8.GetString(e.Body.ToArray()));
        }
    }
}
