using Domain.Entities;
using Domain.Interface;
using MongoDB.Driver;

namespace Infra.Repositories.Implementation;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly IMongoCollection<Motorcycle> _collection;
    
    public MotorcycleRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Motorcycle>("motorcycle");
        var indexKeysDefinition = Builders<Motorcycle>.IndexKeys.Ascending(moto => moto.Plate);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var model = new CreateIndexModel<Motorcycle>(indexKeysDefinition, indexOptions);
        _collection.Indexes.CreateOne(model);
    }

    public async Task AddAsync(Motorcycle moto)
        => await _collection.InsertOneAsync(moto);
    
    public async Task<UpdateResult> UpdatePlateAsync(Guid id, string plate)
    {
        var filter = Builders<Motorcycle>.Filter.Eq(x => x.Id, id);
        var update = Builders<Motorcycle>.Update.Set(x => x.Plate, plate);
        var updateResult = await _collection.UpdateOneAsync(filter, update);
        return updateResult;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteResult = await _collection.DeleteOneAsync(c => c.Id == id);
        return deleteResult.DeletedCount == 1 ? true : false;
    }

    public async Task<Motorcycle> GetByIdAsync(Guid id)
        => await _collection.Find(c => c.Id == id).SingleOrDefaultAsync();

    public async Task<Motorcycle> GetByPlateAsync(string plate)
        =>  await _collection.Find(c => c.Plate == plate).SingleOrDefaultAsync();

    public async Task MarkMotorInUse(Guid motorId, bool inUse)
    {
        var filter = Builders<Motorcycle>.Filter.Eq(x => x.Id, motorId);
        var update = Builders<Motorcycle>.Update.Set(x => x.InUse, inUse);
        var updateResult = await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<bool> MotocycleInUseAsync(Guid motorId)
        => await _collection.Find(c => c.Id == motorId && c.InUse == true).AnyAsync();

    public async Task<IEnumerable<Motorcycle>> GetAllAsync(bool inUse)
        => await _collection.Find(c => true && c.InUse == inUse).ToListAsync();

}
