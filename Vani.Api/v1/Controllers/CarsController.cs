using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vani.Services.Cars;
using Vani.Services.Utils;
using Vani.Shared.DTOS.Cars;
using Vani.Shared.Exceptions;

namespace Vani.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCar(CreateCarDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _carService.CreateCar(model);

        if (result == null)
            return NotFound("Make cannot be found");

        return CreatedAtAction(nameof(GetCarbyId), new { id = result.Id }, GarageMappers.MapCarResponse(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCarbyId(int id)
    {
        var result = await _carService.GetCarbyId(id);

        if (result == null)
            return NotFound();

        return Ok(GarageMappers.MapCarResponse(result));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCars()
    {
        var result = await _carService.GetCars();

        return Ok(GarageMappers.MapCarListResponse(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await _carService.DeleteCar(id);
        if (car == false)
        {
            return NotFound();
        }
        return NoContent();
    }

}
