using System.ComponentModel.DataAnnotations.Schema;

namespace Vani.Domain.Models;

public class Car
{
    public Guid Id { get; set; }	
		
    [ForeignKey("MakeId")]
    public Make Make { get; set; }

    public int MakeId { get; set; }

    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
}
