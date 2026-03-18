using InvestmentService.Application.Commands;
using InvestmentService.Application.DTOs;
using InvestmentService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpGet("{categoryId:int}/subcategories")]
    public async Task<ActionResult<List<SubCategoryDto>>> GetSubCategories(int categoryId)
    {
        var result = await _mediator.Send(new GetSubCategoriesByCategoryQuery(categoryId));
        return Ok(result);
    }

    [HttpPost("subcategories")]
    public async Task<ActionResult<SubCategoryDto>> CreateSubCategory([FromBody] CreateSubCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BrokersController : ControllerBase
{
    private readonly IMediator _mediator;
    public BrokersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<BrokerDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllBrokersQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BrokerDto>> Create([FromBody] CreateBrokerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MasterDataController : ControllerBase
{
    private readonly IMediator _mediator;
    public MasterDataController(IMediator mediator) => _mediator = mediator;

    [HttpGet("credit-agencies")]
    public async Task<ActionResult<List<CreditAgencyDto>>> GetCreditAgencies()
    {
        var result = await _mediator.Send(new GetAllCreditAgenciesQuery());
        return Ok(result);
    }

    [HttpGet("credit-ratings")]
    public async Task<ActionResult<List<CreditRatingDto>>> GetCreditRatings()
    {
        var result = await _mediator.Send(new GetAllCreditRatingsQuery());
        return Ok(result);
    }
}
