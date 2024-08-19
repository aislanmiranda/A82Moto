namespace Domain.Entities;

public class DeliveryMan : EntityBase
{
	public string Name { get; set; }
	public string Cnpj { get; set; }
	public DateTime BirthDate { get; set; }
	public int NumberCnh { get; set; }
	public string TypeCnh { get; set; }
	public string PhotoCnh { get; set; } = "";
	public string FullPath { get; set; } = "";
}
