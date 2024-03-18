using Vani.Shared.DTOS.Cars;

namespace Vani.Shared.DTOS.Makes;

public class MakeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public List<CarResponseDto> Cars { get; set; }
}
