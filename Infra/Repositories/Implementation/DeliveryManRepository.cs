using Domain.Entities;
using Domain.Interface;
using MongoDB.Driver;

namespace Infra.Repositories.Implementation;

public class DeliveryManRepository : IDeliveryManRepository
{
    private readonly IMongoCollection<DeliveryMan> _collection;

    public DeliveryManRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<DeliveryMan>("deliveryman");

        var cnpjIndexKeysDefinition = Builders<DeliveryMan>.IndexKeys.Ascending(del => del.Cnpj);
        var cnpjIndexOptions = new CreateIndexOptions { Unique = true };
        var cnpjIndexModel = new CreateIndexModel<DeliveryMan>(cnpjIndexKeysDefinition, cnpjIndexOptions);
        _collection.Indexes.CreateOne(cnpjIndexModel);

        var cnhIndexKeysDefinition = Builders<DeliveryMan>.IndexKeys.Ascending(del => del.NumberCnh);
        var cnhIndexOptions = new CreateIndexOptions { Unique = true };
        var cnhIndexModel = new CreateIndexModel<DeliveryMan>(cnhIndexKeysDefinition, cnhIndexOptions);
        _collection.Indexes.CreateOne(cnhIndexModel);
    }

    public async Task AddAsync(DeliveryMan deliveryman)
        => await _collection.InsertOneAsync(deliveryman);

    public async Task<DeliveryMan> GetByIdAsync(Guid id)
        => await _collection.Find(c => c.Id == id).SingleOrDefaultAsync();

    public async Task<bool> ExistDeliveryManWithCnpjAndCnh(string cnpj, int cnh)
    {
        var filter = Builders<DeliveryMan>.Filter.And(
            Builders<DeliveryMan>.Filter.Eq(x => x.Cnpj, cnpj),
            Builders<DeliveryMan>.Filter.Eq(x => x.NumberCnh, cnh)
        );

        bool result = await _collection.Find(filter).AnyAsync();

        return result;
    }

    public async Task<bool> UpdatePhotoAsync(Guid id, string file, string pathFull)
    {
        var filter = Builders<DeliveryMan>.Filter.Eq(x => x.Id, id);
        var update = Builders<DeliveryMan>.Update.Set(x => x.PhotoCnh, file)
                                                 .Set(x => x.FullPath, pathFull);
        var updateResult = await _collection.UpdateOneAsync(filter, update);

        return updateResult.ModifiedCount == 1 ? true : false;
    }

    public async Task<IEnumerable<DeliveryMan>> GetAllAsync()
        => await _collection.Find(c => true).ToListAsync();

}