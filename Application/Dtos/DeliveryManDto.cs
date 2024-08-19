using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos;

public class DeliveryManRequestDto
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public int NumberCnh { get; set; }
    public string TypeCnh { get; set; }
}

public class DeliveryManResponseDto : EntityBase
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public int NumberCnh { get; set; }
    public string TypeCnh { get; set; }
    public string PhotoCnh { get; set; } = "";
    public string FullPath { get; set; } = "";
}

public class DeliveryManUpdatePhotoDto
{
    public Guid Id { get; set; }
    public IFormFile? PhotoFile { get; set; }
}