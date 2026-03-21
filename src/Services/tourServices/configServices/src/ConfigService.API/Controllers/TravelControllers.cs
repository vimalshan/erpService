using ConfigService.Application.Features.Travel.Commands;
using ConfigService.Application.Features.Travel.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CountriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllCountriesQuery(), ct));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCountryByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCountryCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CountryId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCountryCommand command, CancellationToken ct)
    {
        if (id != command.CountryId) return BadRequest("ID mismatch.");
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct) =>
        await mediator.Send(new DeleteCountryCommand(id), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitiesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllCitiesQuery(), ct));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCityByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("country/{countryId}")]
    public async Task<IActionResult> GetByCountry(string countryId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetCitiesByCountryQuery(countryId), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCityCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CityId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCityCommand command, CancellationToken ct)
    {
        if (id != command.CityId) return BadRequest("ID mismatch.");
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct) =>
        await mediator.Send(new DeleteCityCommand(id), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TravelClassesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllTravelClassesQuery(), ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TravelContactsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllTravelContactsQuery(), ct));
}
