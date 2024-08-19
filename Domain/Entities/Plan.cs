namespace Domain.Entities;

public sealed class Plan : EntityBase
{
    public Plan(string label, decimal price, int period, decimal valueFine, decimal additionalDailyValue)
    {
        Label = label;
        Price = price;
        Period = period;
        ValueFine = valueFine;
        AdditionalDailyValue = additionalDailyValue;
    }

    public string Label { get; private set; }
    public decimal Price { get; private set; }
    public int Period { get; private set; }
    public decimal ValueFine { get; private set; }
    public decimal AdditionalDailyValue { get; private set; }
}