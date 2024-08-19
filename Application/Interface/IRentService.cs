
using Application.Dtos;

namespace Application.Interface;

public interface IRentService
{
    Task AddAsync(RentRequestDto dto);
    Task<IEnumerable<RentResponseDto>> GetAllAsync();
    Task<BudGetResponseDto> ConsultBudgetAsync(BudGetRequestDto dto);
}
