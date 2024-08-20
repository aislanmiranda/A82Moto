
using Domain.Entities;

namespace Infra.Messaging;

public interface IRabbitMQService
{
	void PublishMessage(Notification data);	
}
