using Domain.Entities;

namespace Domain.Interface;
	
public interface INotificationRepository
{
    Task AddAsync(Notification notification);
}