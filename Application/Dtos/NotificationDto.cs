namespace Application.Dtos;

public class NotificationRequestDto
{
	public NotificationRequestDto()
	{

	}

    public string EventName { get; set; }
    public string Message { get; set; }
    public DateTime SendDate { get; set; } = DateTime.Now;
}
