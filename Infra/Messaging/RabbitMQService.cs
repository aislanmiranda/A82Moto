using System.Text;
using Domain.Entities;
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
		    HostName = "rabbit-prod"
	    };

	    _connection = connectionFactory.CreateConnection(_connectionName);
	    _channel = _connection.CreateModel();
    
        _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);

        _channel.QueueDeclare(_queueName, true, false, false, null);

        _channel.QueueBind(_queueName, _exchange, _routingKey);
    }

    public void PublishMessage(Notification data)
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
