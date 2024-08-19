namespace Application.Dtos;

public class MotorcycleRequestDto
{
    public int Year { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }
}

public class MotorcycleResponseDto
{
    public Guid Id { get; private set; }
    public int Year { get; private set; }
    public string Model { get; private set; }
    public string Plate { get; private set; }
    public bool InUse { get; private set; }
}

public class MotocycleUpdatePlateDto
{
    public Guid Id { get; set; }
    public string Plate { get; set; }
}