using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interface;

public interface IDeliveryManRepository
{
    Task AddAsync(DeliveryMan deliveryman);

    Task<DeliveryMan> GetByIdAsync(Guid id);

    Task<bool> ExistDeliveryManWithCnpjAndCnh(string cnpj, int cnh);

    Task<bool> UpdatePhotoAsync(Guid id, string file, string pathFull);

    Task<IEnumerable<DeliveryMan>> GetAllAsync();
}
