using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RentMapper : Profile
{
	public RentMapper()
	{
		CreateMap<RentRequestDto, BudGetRequestDto>();
		CreateMap<Rent, RentResponseDto>();

        CreateMap<BudGetResponseDto, Rent>()
            .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId));
    }
}
