using Domain.Entities;

namespace Application.Dtos;

public class PlanRequestCreateDto 
{
    public string Label { get; set; }
    public int Period { get; set; }
    public decimal Price { get; set; }
    public decimal ValueFine { get; set; }
}

public class PlanRequestUpdateDto : EntityBase
{
    public string Label { get; set; }
    public int Period { get; set; }
    public decimal Price { get; set; }
    public decimal ValueFine { get; set; }
    public decimal AdditionalDailyValue { get; set; }
}