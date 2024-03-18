using System.ComponentModel.DataAnnotations.Schema;

namespace Vani.Domain.Models;

public class Make
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public List<Car> Cars { get; set; } = new List<Car>();
}
