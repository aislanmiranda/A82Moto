using Domain.Entities;

namespace Domain.Interface;

public interface IRentRepository
{
    Task AddAsync(Rent rent);

    Task<IEnumerable<Rent>> GetAllAsync();
}
