using Application.Dtos;

namespace Application.Interface;

public interface IDeliveryManService
{
    Task AddAsync(DeliveryManRequestDto dto);

    Task UpdatePhotoAsync(DeliveryManUpdatePhotoDto dto);

    Task<IEnumerable<DeliveryManResponseDto>> GetAll();

}