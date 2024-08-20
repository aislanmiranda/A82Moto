using Domain.Entities;
using Domain.Interface;
using MongoDB.Driver;

namespace Infra.Repositories.Implementation
{
	public class NotificationRepository : INotificationRepository
    {
		
        private readonly IMongoCollection<Notification> _collection;

        public NotificationRepository(IMongoDatabase database)
            => _collection = database.GetCollection<Notification>("notification");

        public async Task AddAsync(Notification notification)
            => await _collection.InsertOneAsync(notification);
	}
}

