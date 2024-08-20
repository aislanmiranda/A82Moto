namespace Domain.Entities;

public class Notification : EntityBase
{
    public Notification(string eventName, string message, DateTime sendDate)
    {
        EventName = eventName;
        Message = message;
        SendDate = sendDate;
    }

    public string EventName { get; private set; }
    public string Message { get; private set; }   
    public DateTime SendDate { get; private set; }
}