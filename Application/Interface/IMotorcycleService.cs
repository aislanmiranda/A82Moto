
using Application.Dtos;

namespace Application.Interface;

public interface IMotorcycleService
{
    Task AddAsync(MotorcycleRequestDto model);
    Task UpdatePlateAsync(MotocycleUpdatePlateDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<MotorcycleResponseDto>> GetAllAsync(bool inUse);
    Task<MotorcycleResponseDto> GetByPlateAsync(string plate);
}