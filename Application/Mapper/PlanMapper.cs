using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class PlanMapper : Profile
{
	public PlanMapper()
	{
		CreateMap<Plan, PlanRequestUpdateDto>()
			.ReverseMap();
        
    }
}
