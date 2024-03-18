using Vani.Domain.Models;
using Vani.Shared.DTOS.Cars;
using Vani.Shared.DTOS.Makes;

namespace Vani.Services.Utils;

public static class GarageMappers
{
    public static List<CarResponseDto> MapCarListResponse(IEnumerable<Car> cars)
    {
        List<CarResponseDto> carResponseDtos = new List<CarResponseDto>();
        foreach (var car in cars)
        {
            carResponseDtos.Add(MapCarResponse(car));
        }

        return carResponseDtos;
    }

    public static CarResponseDto MapCarResponse(Car car)
    {
        return new CarResponseDto
        {
            Id = car.Id,
            Make = car.Make.Name,
            Model = car.Model,
            Year = car.Year,
            Color = car.Color,
            Mileage = car.Mileage,
            DateTimeCreated = car.DateTimeCreated,
            LastModifiedDateTime = car.LastModifiedDateTime
        };
    }
    
    public static MakeResponseDto MapMakeResponse(Make make)
    {
        return new MakeResponseDto
        {
            Id = make.Id,
            Name = make.Name,
            Country = make.Country,
            Cars = GarageMappers.MapCarListResponse(make.Cars)
        };
    }

}
