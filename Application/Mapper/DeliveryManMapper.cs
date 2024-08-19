using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
	public class DeliveryManMapper : Profile
	{
		public DeliveryManMapper()
		{
			CreateMap<DeliveryMan, DeliveryManRequestDto>()
				.ReverseMap();

            CreateMap<DeliveryMan, DeliveryManResponseDto>()
                .ReverseMap();
        }
	}
}

