using Application.Dtos;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Domain.Interface;

namespace Application.Implementation;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;

    public PlanService(IPlanRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //public async Task AddAsync(PlanRequestCreateDto dto)
    //{
    //    try
    //    {
    //        var plan = _mapper.Map<Plan>(dto);
    //        await _repository.AddAsync(plan);
    //    }
    //    catch (Exception ex)
    //    {
    //       throw new Exception(ex.Message);
    //    }
    //}

    public async Task<IEnumerable<PlanRequestUpdateDto>> GetAllAsync()
    {
        try
        {
            var model = await _repository.GetAllAsync();

            var results = _mapper.Map<IEnumerable<PlanRequestUpdateDto>>(model);

            return results;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
