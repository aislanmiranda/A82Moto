using Application.Dtos;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Messages;
using Infra.Messaging;

namespace Application.Implementation;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRabbitMQService _messageBus;

    public MotorcycleService(IMotorcycleRepository repository,
        IMapper mapper, IRabbitMQService messageBus)
    {
        _repository = repository;
        _mapper = mapper;
        _messageBus = messageBus;
    }

    public async Task AddAsync(MotorcycleRequestDto dto)
    {
        try
        {   
            var existPlate = await _repository.GetByPlateAsync(dto.Plate);
            if(existPlate != null)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

            var motocycle = _mapper.Map<Motorcycle>(dto);

            await _repository.AddAsync(motocycle);

            if(dto.Year == 2024)
            {
                _messageBus.PublishMessage(new
                {
                    Nome = "Creted Motorcycle",
                    EventName = $"Creted Motorcycle {dto.Plate} year {dto.Year}"
                });
            }
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task UpdatePlateAsync(MotocycleUpdatePlateDto dto)
    {
        try
        {
            var result = await _repository.UpdatePlateAsync(dto.Id, dto.Plate);

            if (result.MatchedCount == 0)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_404, ExceptionMessage.ERROR_404_MSG);
            if (result.ModifiedCount == 0)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            if (await MotocycleIsRentendAsync(id))
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

            var result = await _repository.DeleteAsync(id);

            if (!result)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_404, ExceptionMessage.ERROR_404_MSG);

        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task<IEnumerable<MotorcycleResponseDto>> GetAllAsync(bool inUse)
    {
        try
        {
            var model = await _repository.GetAllAsync(inUse);

            var results = _mapper.Map<IEnumerable<MotorcycleResponseDto>>(model);

            return results;
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task<MotorcycleResponseDto> GetByPlateAsync(string plate)
    {
        try
        {
            var model = await _repository.GetByPlateAsync(plate);

            var dto = _mapper.Map<MotorcycleResponseDto>(model);

            return dto;
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    private async Task<bool> MotocycleIsRentendAsync(Guid motorId)
        =>  await _repository.MotocycleInUseAsync(motorId);
        
    
}
