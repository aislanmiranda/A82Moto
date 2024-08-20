using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class NotificationMapper : Profile
{
	public NotificationMapper()
	{
		CreateMap<NotificationRequestDto, Notification>();
    }
}
