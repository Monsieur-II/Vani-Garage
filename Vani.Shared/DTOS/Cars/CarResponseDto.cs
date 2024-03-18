namespace Vani.Shared.DTOS.Cars;

public class CarResponseDto
{
    public Guid Id { get; set; }	

    public string Make { get; set; }
	
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
}
