
using Application.Dtos;

namespace Application.Interface;

public interface IPlanService
{
    //Task AddAsync(PlanRequestCreateDto dto);
    Task<IEnumerable<PlanRequestUpdateDto>> GetAllAsync();
}
