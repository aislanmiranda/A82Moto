
namespace Application.Dtos;

public class BudGetRequestDto
{
    public Guid PlanId { get; set; }
    public DateTime ForecastDate { get; set; }
}

public class BudGetResponseDto
{
    public BudGetResponseDto(
        Guid planId,
        string planDescription,
        decimal planPrice,
        DateTime startDate,
        DateTime endDate,
        DateTime forecastDate,
        decimal subTotalRent,
        decimal fineValue,
        decimal totalRent,
        string description
        )
    {
        PlanId = planId;
        PlanDescription = planDescription;
        PlanPrice = planPrice;
        StartDate = startDate.ToString("yyyy-MM-dd");
        EndDate = endDate.ToString("yyyy-MM-dd");
        ForecastDate = forecastDate.ToString("yyyy-MM-dd");
        SubTotalRent = subTotalRent;
        FineValue = fineValue;
        TotalRent = totalRent;
        Description = description;
    }

    public Guid PlanId { get; private set; }
    public string PlanDescription { get; private set; }
    public decimal PlanPrice { get; private set; }

    public string StartDate { get; private set; }
    public string EndDate { get; private set; }
    public string ForecastDate { get; private set; }
    public decimal SubTotalRent { get; private set; }
    public decimal FineValue { get; private set; }
    public decimal TotalRent { get; private set; }
    public string Description { get; private set; } = "";
}