using Microsoft.EntityFrameworkCore;
using Vani.Domain.Models;
using Vani.Infras;
using Vani.Services.Cache;
using Vani.Shared.DTOS.Cars;

namespace Vani.Services.Cars;

public class CarService : ICarService
{
    private VaniDbContext _context;
    private readonly ICacheService _cacheService;

    public CarService(VaniDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<Car?> CreateCar(CreateCarDto carDto)
    {
        var make = await GetMake(carDto.Make);
        if (make != null)
        {
            var newCar = new Car
            {
                Id = Guid.NewGuid(),
                Make = make,
                MakeId = make.Id,
                Model = carDto.Model,
                Year = carDto.Year,
                Color = carDto.Color,
                Mileage = carDto.Mileage,
                DateTimeCreated = DateTime.UtcNow,
                LastModifiedDateTime = DateTime.UtcNow
            };
            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();
            _cacheService.RemoveData("cars");
            return newCar;
        }

        return null;
    }

    public async Task<Car?> GetCarbyId(int id)
    {
        return await _context.Cars.FindAsync(id);
    }

    public async Task<IEnumerable<Car>> GetCars()
    {
        var cachedResult = _cacheService.GetData<Car>("cars");

        if (cachedResult != null)
            return cachedResult;

        var contextResult = await _context.Cars.Include(c => c.Make).ToListAsync();

        _cacheService.SetData("cars", contextResult, DateTimeOffset.Now.AddHours(5));

        return contextResult;
    }

    public async Task<Make?> GetMake(string make)
    {
        return await _context.Makes.FirstOrDefaultAsync(m => m.Name == make);
    }
    
    public Task<Car> UpdateCar(int id, CreateCarDto car)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> DeleteCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            _cacheService.RemoveData("cars");
            return true;
        }

        return false;
    }
}
