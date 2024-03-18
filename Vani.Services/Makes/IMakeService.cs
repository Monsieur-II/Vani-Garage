using Vani.Domain.Models;
using Vani.Shared.DTOS.Makes;

namespace Vani.Services.Makes;

public interface IMakeService
{
    Task<Make> CreateMake(CreateMakeDto make);
    Task<Make?> GetMakebyId(int id);        
    Task<IEnumerable<Make>> GetMakes();
}
