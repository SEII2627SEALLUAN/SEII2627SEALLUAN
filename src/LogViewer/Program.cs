using Microsoft.Extensions.Configuration;

namespace LogViewer;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        Console.WriteLine(
            $"Config: {configuration["RabbitMQ:HostName"]}:{configuration["RabbitMQ:Port"]}, " +
            $"exchange '{configuration["RabbitMQ:Exchange"]}'");

        var subscriber = new Subscriber(configuration);
        await subscriber.StartConsuming();

        Console.WriteLine("LogViewer listo. Pulsa una tecla para salir.");
        Console.ReadKey();
    }
}
