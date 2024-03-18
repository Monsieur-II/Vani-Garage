using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vani.Services.Makes;
using Vani.Services.Utils;
using Vani.Shared.DTOS.Makes;
using Vani.Shared.Exceptions;

namespace Vani.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MakesController : ControllerBase
{
    private readonly IMakeService _makeService;

    public MakesController(IMakeService makeService)
    {
        _makeService = makeService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> CreateMake(CreateMakeDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _makeService.CreateMake(model);

        return CreatedAtAction(nameof(GetMake), new { id = result.Id }, GarageMappers.MapMakeResponse(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> GetMake(int id)
    {
        var result = await _makeService.GetMakebyId(id);

        if (result == null)
            return NotFound();

        return Ok(GarageMappers.MapMakeResponse(result));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> GetMakes()
    {
        var makes = await _makeService.GetMakes();

        List<MakeResponseDto> result = new List<MakeResponseDto>();

        foreach (var make in makes)
        {
            result.Add(GarageMappers.MapMakeResponse(make));
        }

        return Ok(result);
    }
}
