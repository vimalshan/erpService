using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using travelTransactionService.Application.Commands;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Application.Queries;

namespace travelTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VendorMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllVendorsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{vendorId}")]
    [ProducesResponseType(typeof(VendorMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long vendorId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVendorByIdQuery(vendorId), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("category/{categoryType}")]
    [ProducesResponseType(typeof(IReadOnlyList<VendorMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(string categoryType, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVendorsByCategoryQuery(categoryType), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(VendorMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVendorCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { vendorId = result.VendorId }, result);
    }

    [HttpPut("{vendorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long vendorId, [FromBody] UpdateVendorCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { VendorId = vendorId };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }

    [HttpDelete("{vendorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long vendorId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteVendorCommand(vendorId), cancellationToken);
        return Ok(new { Success = result });
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaxMastersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxMastersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TaxMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTaxMastersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{taxType}")]
    [ProducesResponseType(typeof(TaxMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByType(string taxType, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaxMasterByTypeQuery(taxType), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("vendor/{vendorId}")]
    [ProducesResponseType(typeof(IReadOnlyList<TaxMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByVendor(long vendorId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaxMastersByVendorQuery(vendorId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaxMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaxMasterCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }

    [HttpPut("{taxType}/rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRate(string taxType, [FromBody] UpdateTaxRateCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { TaxType = taxType };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JaiInterfaceLinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public JaiInterfaceLinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<JaiInterfaceLineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllJaiInterfaceLinesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{interfaceLineId}")]
    [ProducesResponseType(typeof(JaiInterfaceLineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal interfaceLineId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetJaiInterfaceLineByIdQuery(interfaceLineId), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("batch/{batchId}")]
    [ProducesResponseType(typeof(IReadOnlyList<JaiInterfaceLineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBatch(decimal batchId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetJaiInterfaceLinesByBatchQuery(batchId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(JaiInterfaceLineDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateJaiInterfaceLineCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }

    [HttpPut("{interfaceLineId}/gst")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGst(decimal interfaceLineId, [FromBody] UpdateGstAmountsCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { InterfaceLineId = interfaceLineId };
        var result = await _mediator.Send(updatedCommand, cancellationToken);
        return Ok(new { Success = result });
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionLookupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionLookupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("account-masters")]
    [ProducesResponseType(typeof(IReadOnlyList<AccountMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccountMasters(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllAccountMastersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("gl-code-combinations")]
    [ProducesResponseType(typeof(IReadOnlyList<GlCodeCombinationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGlCodeCombinations(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllGlCodeCombinationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("jv-interfaces")]
    [ProducesResponseType(typeof(IReadOnlyList<JvInterfaceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJvInterfaces(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllJvInterfacesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("jv-missing-combicodes")]
    [ProducesResponseType(typeof(IReadOnlyList<JvMissingCombiCodeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJvMissingCombiCodes(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllJvMissingCombiCodesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("batch-sub-breakups")]
    [ProducesResponseType(typeof(IReadOnlyList<BatchSubBreakupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBatchSubBreakups(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllBatchSubBreakupsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("travel-ap-params")]
    [ProducesResponseType(typeof(IReadOnlyList<TravelApParamsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTravelApParams(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTravelApParamsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("travel-ap-params/{apUnitId}")]
    [ProducesResponseType(typeof(TravelApParamsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTravelApParamsById(long apUnitId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTravelApParamsByIdQuery(apUnitId), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("source-history")]
    [ProducesResponseType(typeof(IReadOnlyList<SourceHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSourceHistory(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSourceHistoryQuery(), cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly travelTransactionService.Domain.Interfaces.IBlobStorageService _blobStorage;

    public BlobStorageController(travelTransactionService.Domain.Interfaces.IBlobStorageService blobStorage)
    {
        _blobStorage = blobStorage;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string container = "transaction-documents", CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobStorage.UploadFileAsync(container, fileName, stream, file.ContentType, cancellationToken);

        return Ok(new { Url = url, FileName = fileName });
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string container, [FromQuery] string fileName, CancellationToken cancellationToken)
    {
        var stream = await _blobStorage.DownloadFileAsync(container, fileName, cancellationToken);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string container, [FromQuery] string fileName, CancellationToken cancellationToken)
    {
        var result = await _blobStorage.DeleteFileAsync(container, fileName, cancellationToken);
        return result ? Ok() : NotFound();
    }
}
