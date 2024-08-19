
namespace Infra.Messaging;

public interface IRabbitMQService
{
	void PublishMessage(object data);	
}
