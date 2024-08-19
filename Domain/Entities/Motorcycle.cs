namespace Domain.Entities;

public sealed class Motorcycle : EntityBase
{
	public Motorcycle(int year, string model, string plate)
	{
		Year = year;
		Model = model;
		Plate = plate;
	}

    public Motorcycle(string plate)
    {
        Plate = plate;
    }

    public int Year { get; private set; }
    public string Model { get; private set; }
    public string Plate { get; private set; }
    public bool InUse { get; private set; } = false;
}