
namespace Domain.Events;

public class MotorcycleCreatedEvent
{
	

    public MotorcycleCreatedEvent(string eventName, string message)
    {
        EventName = eventName;
        Message = message;
    }

    public string EventName { get; private set; }
    public string Message { get; private set; }
}