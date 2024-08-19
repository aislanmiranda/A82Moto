using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infra.Messaging;

public class RabbitMQService : IRabbitMQService, IDisposable
{
private readonly IConnection _connection;
private readonly IModel _channel;
private const string _exchange = "notification-system";
private const string _queueName = "motorcycle-event";
private const string _routingKey = "notification-key";
private const string _connectionName = "notification-system-publisher";
private bool _disposed = false;

public RabbitMQService()
{
	var connectionFactory = new ConnectionFactory
	{
		HostName = "localhost"
	};
	_connection = connectionFactory.CreateConnection(_connectionName);
	_channel = _connection.CreateModel();

    // Configurando o Exchange do tipo 'fanout' para distribuir mensagens a todos os consumidores
    _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);

    // Declarando a fila
    _channel.QueueDeclare(_queueName, true, false, false, null);

    // Ligando a fila ao Exchange usando a chave de roteamento
    _channel.QueueBind(_queueName, _exchange, _routingKey);
}

public void PublishMessage(object data)
{
	var payload = JsonConvert.SerializeObject(data);
	var bytearray = Encoding.UTF8.GetBytes(payload);
	_channel.BasicPublish(_exchange, _routingKey, null, bytearray);
}

public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}

protected virtual void Dispose(bool disposing)
{
    if (_disposed)
        return;

    if (disposing)
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }

    _disposed = true;
}

~RabbitMQService()
{
    Dispose(false);
}
}
