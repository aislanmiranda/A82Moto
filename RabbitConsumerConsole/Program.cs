using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    private const string _connectionName = "notification-system-consumer";
    private const string _exchange = "notification-system";

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection(_connectionName))
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: "");

            Console.WriteLine("[*] Waiting for messages in queue: {0}.", queueName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("[x] Received {0}", message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
