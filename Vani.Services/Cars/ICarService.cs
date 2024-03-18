using Vani.Domain.Models;
using Vani.Shared.DTOS.Cars;

namespace Vani.Services.Cars;

public interface ICarService
{
    Task<IEnumerable<Car>> GetCars();
    Task<Car?> GetCarbyId(int id);	
		
    Task<Make?> GetMake(string make);
    Task<Car?> CreateCar(CreateCarDto carDto);
    Task<Car> UpdateCar(int id, CreateCarDto car);
    Task<bool> DeleteCar(int id);
}
