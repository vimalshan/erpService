using MasterService.Application.Features.Categories.Queries;
using MasterService.Application.Features.FinancialYears.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetCategoriesQuery(), ct));

    [HttpGet("{categoryCode}")]
    public async Task<IActionResult> GetById(string categoryCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryByCodeQuery(categoryCode), ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[ApiController]
[Route("api/financial-years")]
[Authorize]
public class FinancialYearsController(IMediator mediator) : ControllerBase
{
    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken ct)
        => Ok(await mediator.Send(new GetActiveFinancialYearsQuery(), ct));
}
