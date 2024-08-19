using System;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
	public class MotocycleMapper : Profile
	{
		public MotocycleMapper()
		{
			CreateMap<MotorcycleRequestDto, Motorcycle>();

			CreateMap<Motorcycle, MotorcycleResponseDto>();

            CreateMap<MotocycleUpdatePlateDto, Motorcycle>();

            // exemplo para mapear o valor de um enum para string
            //.ForMember(p => p.MaritalStatus, options => options.MapFrom(x => x.MaritalStatus.ToString()));
        }
	}
}

