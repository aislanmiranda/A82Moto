using Domain.Entities;
using Domain.Interface;
using MongoDB.Driver;

namespace Infra.Repositories.Implementation;

public class RentRepository : IRentRepository
{
    private readonly IMongoCollection<Rent> _collection;

    public RentRepository(IMongoDatabase database)
        => _collection = database.GetCollection<Rent>("rent");

    public async Task AddAsync(Rent Rent)
        => await _collection.InsertOneAsync(Rent);
   
    public async Task<IEnumerable<Rent>> GetAllAsync()
        => await _collection.Find(c => true).ToListAsync();
    
}
