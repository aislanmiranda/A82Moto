using Application.Dtos;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Messages;

namespace Application.Implementation;

public class RentService : IRentService
{
    private readonly IRentRepository _repository;
    private readonly IPlanRepository _planRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IDeliveryManRepository _deliveriManRepository;
    private readonly IMapper _mapper;

    public RentService(IRentRepository repository,
        IPlanRepository planRepository,
        IMotorcycleRepository motorcycleRepository,
        IDeliveryManRepository deliveriManRepository,
        IMapper mapper)
    {
        _repository = repository;
        _planRepository = planRepository;
        _motorcycleRepository = motorcycleRepository;
        _deliveriManRepository = deliveriManRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(RentRequestDto dto)
    {
        try
        {
            if(!await CheckCNHType(dto.DeliveryManId))
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

            BudGetRequestDto dataRent = _mapper.Map<BudGetRequestDto>(dto);

            BudGetResponseDto calculate = await ConsultBudgetAsync(dataRent);

            Rent rent = _mapper.Map<Rent>(calculate);
            rent.MotocycleId = dto.MotocycleId;
            rent.DeliveryManId = dto.DeliveryManId;
            
            await _repository.AddAsync(rent);

            await _motorcycleRepository.MarkMotorInUse(dto.MotocycleId, true);
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    private async Task<bool> CheckCNHType(Guid deliveryManId)
    {
        var deliveryMan = await _deliveriManRepository.GetByIdAsync(deliveryManId);

        return deliveryMan.TypeCnh == "A" ? true : false;
    }

    public async Task<BudGetResponseDto> ConsultBudgetAsync(BudGetRequestDto dto)
    {
        try
        {
            Plan plan = await _planRepository.GetByIdAsync(dto.PlanId);

            DateTime startDate = DateTime.Now.Date;
            DateTime forecastDate = dto.ForecastDate.Date; //data prevista para entrega
            int daysPlan = plan.Period;
            int daysForecast = (forecastDate - startDate).Days;
            decimal subTotalRent = daysPlan * plan.Price;
            decimal fineValue = 0.00M;
            decimal totalRent = 0.00M;
            string description = string.Empty;

            if (daysForecast < daysPlan)
            {
                int daysNotEffective = daysPlan - daysForecast;
                decimal valueFine = (plan.Price * daysNotEffective) * plan.ValueFine;
                fineValue = valueFine; //valor multa
                description = $"Valor com multa de {plan.ValueFine}% para {daysNotEffective} dia(s) não efetivado(s)";
            }
            else if (daysForecast > daysPlan)
            {
                int additionalDaysEffective = daysForecast - daysPlan;
                decimal valueAdditional = additionalDaysEffective * plan.AdditionalDailyValue;
                fineValue = valueAdditional;
                description = $"Valor adicional de {plan.AdditionalDailyValue} para {additionalDaysEffective} dia(s) adicional(is)";
            }

            totalRent = fineValue > 0.00M ? (fineValue + subTotalRent) : subTotalRent;

            BudGetResponseDto budget = new (
                planId: dto.PlanId,
                planDescription: plan.Label,
                planPrice: plan.Price,
                startDate: startDate.AddDays(1),
                endDate: startDate.AddDays(daysPlan),
                forecastDate: forecastDate,
                subTotalRent: subTotalRent,
                fineValue: fineValue,
                totalRent: totalRent,
                description: description
            );

            return budget;

        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task<IEnumerable<RentResponseDto>> GetAllAsync()
    {
        try
        {
            var model = await _repository.GetAllAsync();

            var results = _mapper.Map<IEnumerable<RentResponseDto>>(model);

            return results;
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

}
