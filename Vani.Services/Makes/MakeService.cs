using Microsoft.EntityFrameworkCore;
using Vani.Infras;
using Vani.Services.Cache;
using Vani.Domain.Models;
using Vani.Services.Kafka;
using Vani.Shared;
using Vani.Shared.DTOS.Makes;

namespace Vani.Services.Makes;

public class MakeService : IMakeService
{
    private readonly VaniDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IKafkaService _kafkaService;

    public MakeService(VaniDbContext context, ICacheService cacheService, IKafkaService kafkaService)
    {
        _context = context;
        _cacheService = cacheService;
        _kafkaService = kafkaService;
    }

    public async Task<Make> CreateMake(CreateMakeDto make)
    {
        var newMake = new Make { Name = make.Name, Country = make.Country };
        await _context.Makes.AddAsync(newMake);
        await _context.SaveChangesAsync();
        _cacheService.RemoveData("makes");

        return newMake;
    }

    public async Task<Make?> GetMakebyId(int id)
    {
        return await _context.Makes.Include(m => m.Cars).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Make>> GetMakes()
    {
        var makes = await _context.Makes.Include(m => m.Cars).ToListAsync();
        await _kafkaService.ProduceMessageAsync(Constants.GarageTopic, "Makes retrieved");
        

        return makes;
    }
}
