using System.ComponentModel.DataAnnotations;

namespace Vani.Shared.DTOS.Makes;

public class CreateMakeDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Country { get; set; }
}
