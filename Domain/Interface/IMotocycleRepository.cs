
using Domain.Entities;
using MongoDB.Driver;

namespace Domain.Interface;

public interface IMotorcycleRepository
{
    Task AddAsync(Motorcycle moto);

    Task<UpdateResult> UpdatePlateAsync(Guid id, string plate);

    Task<bool> DeleteAsync(Guid id);

    Task<Motorcycle> GetByIdAsync(Guid id);

    Task<Motorcycle> GetByPlateAsync(string plate);

    Task MarkMotorInUse(Guid motorId, bool inUse);

    Task<bool> MotocycleInUseAsync(Guid motorId);

    Task<IEnumerable<Motorcycle>> GetAllAsync(bool inUse);
}


