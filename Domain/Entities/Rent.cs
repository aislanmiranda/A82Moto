namespace Domain.Entities;

public sealed class Rent : EntityBase
{
    public Guid MotocycleId { get; set; }
    public Guid DeliveryManId { get; set; }

    public Guid PlanId { get; private set; }
    public string PlanDescription { get; private set; }
    public decimal PlanPrice { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime ForecastDate { get; private set; }

    public decimal SubTotalRent { get; private set; }
    public decimal FineValue { get; private set; }
    public decimal TotalRent { get; private set; }
    public string Description { get; private set; }

}