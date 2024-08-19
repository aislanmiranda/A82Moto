using Domain.Entities;

namespace Domain.Interface;

public interface IPlanRepository
{
    //Task AddAsync(Plan plan);

    Task<Plan> GetByIdAsync(Guid id);

    Task<IEnumerable<Plan>> GetAllAsync();
}
