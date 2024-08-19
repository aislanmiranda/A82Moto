using Domain.Entities;
using Domain.Interface;
using MongoDB.Driver;

namespace Infra.Repositories.Implementation;

public class PlanRepository : IPlanRepository
{
    private readonly IMongoCollection<Plan> _collection;

    public PlanRepository(IMongoDatabase database)
        => _collection = database.GetCollection<Plan>("plan");

    public async Task<Plan> GetByIdAsync(Guid id)
        => await _collection.Find(c => c.Id == id).SingleOrDefaultAsync();

    public async Task<IEnumerable<Plan>> GetAllAsync()
        => await _collection.Find(c => true).ToListAsync();
}
