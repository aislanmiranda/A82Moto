namespace Application.Dtos;

public class NotificationRequestDto
{
	public NotificationRequestDto()
	{
        SendDate = TimeZoneInfo.ConvertTime(DateTime.Now,
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }

    public string EventName { get; set; }
    public string Message { get; set; }
    public DateTime SendDate { get; set; }

}
