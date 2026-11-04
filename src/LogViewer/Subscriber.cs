using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogViewer;

public class Subscriber
{
    private readonly IConfiguration _configuration;

    public Subscriber(IConfiguration configuration) => _configuration = configuration;

    public async Task StartConsuming()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.TryParse(_configuration["RabbitMQ:Port"], out int p) ? p : 5672,
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var exchange = _configuration["RabbitMQ:Exchange"] ?? "logs";
        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true, autoDelete: false);

        var queueName = (await channel.QueueDeclareAsync()).QueueName;
        await channel.QueueBindAsync(queueName, exchange, routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var log = JsonSerializer.Deserialize<Log>(Encoding.UTF8.GetString(ea.Body.ToArray()));
            Console.WriteLine($"[{log?.Timestamp.ToLocalTime():HH:mm:ss}] [{log?.LogLevel}] {log?.Category}: {log?.Message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
    }
}
