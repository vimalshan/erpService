using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleTracking.Application.DTOs;
using VehicleTracking.Application.Vehicles.Commands;
using VehicleTracking.Application.Vehicles.Queries;

namespace VehicleTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleMasterDto>>> GetAll()
        => Ok(await mediator.Send(new GetAllVehiclesQuery()));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleMasterDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetVehicleByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleMasterDto>> Register([FromBody] RegisterVehicleCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.SerialNumber }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<VehicleMasterDto>> Update(long id, [FromBody] UpdateVehicleMasterCommand command)
    {
        if (id != command.SerialNumber)
            return BadRequest("Route id does not match body SerialNumber.");
        return Ok(await mediator.Send(command));
    }

    [HttpGet("{trackingNumber:long}/stages")]
    public async Task<ActionResult<IEnumerable<VehicleStageDto>>> GetStages(long trackingNumber)
        => Ok(await mediator.Send(new GetVehicleStagesQuery(trackingNumber)));

    [HttpPost("stages")]
    public async Task<ActionResult<VehicleStageDto>> UpdateStage([FromBody] UpdateVehicleStageCommand command)
        => Ok(await mediator.Send(command));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<VehicleTransactionDto>>> GetActive()
        => Ok(await mediator.Send(new GetActiveTransactionsQuery()));

    [HttpGet("{trackingNumber:long}")]
    public async Task<ActionResult<IEnumerable<VehicleTransactionDto>>> GetByTracking(long trackingNumber)
        => Ok(await mediator.Send(new GetVehicleTransactionsQuery(trackingNumber)));

    [HttpPost]
    public async Task<ActionResult<VehicleTransactionDto>> Create([FromBody] CreateVehicleTransactionCommand command)
        => Ok(await mediator.Send(command));

    [HttpPost("{trackingNumber:long}/close")]
    public async Task<ActionResult<VehicleTransactionDto>> Close(long trackingNumber, [FromBody] CloseVehicleTransactionCommand command)
    {
        if (trackingNumber != command.TrackingNumber)
            return BadRequest("Route trackingNumber does not match body.");
        return Ok(await mediator.Send(command));
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{trackingNumber:long}")]
    public async Task<ActionResult<IEnumerable<VehicleInvoiceDto>>> GetByTracking(long trackingNumber)
        => Ok(await mediator.Send(new GetVehicleInvoicesQuery(trackingNumber)));

    [HttpPost]
    public async Task<ActionResult<VehicleInvoiceDto>> Create([FromBody] CreateVehicleInvoiceCommand command)
        => Ok(await mediator.Send(command));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DecisionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{trackingNumber:long}")]
    public async Task<ActionResult<IEnumerable<DecisionFlagDto>>> GetByTracking(long trackingNumber)
        => Ok(await mediator.Send(new GetDecisionFlagsQuery(trackingNumber)));

    [HttpPost]
    public async Task<ActionResult<DecisionFlagDto>> MakeDecision([FromBody] MakeDecisionCommand command)
        => Ok(await mediator.Send(command));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StagesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StageMasterDto>>> GetAll()
        => Ok(await mediator.Send(new GetAllStagesQuery()));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurposesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurposeMasterDto>>> GetAll()
        => Ok(await mediator.Send(new GetAllPurposesQuery()));

    [HttpGet("{purposeCode:long}")]
    public async Task<ActionResult<PurposeMasterDto>> GetWithStages(long purposeCode)
    {
        var result = await mediator.Send(new GetPurposeWithStagesQuery(purposeCode));
        return result is null ? NotFound() : Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{trackingNumber:long}")]
    public async Task<ActionResult<WeightInfoDto>> Get(long trackingNumber)
    {
        var result = await mediator.Send(new GetWeightInfoQuery(trackingNumber));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WeightInfoDto>> Update([FromBody] UpdateWeightInfoCommand command)
        => Ok(await mediator.Send(command));
}
