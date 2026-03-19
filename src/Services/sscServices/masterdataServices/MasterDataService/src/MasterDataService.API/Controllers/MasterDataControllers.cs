using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MasterDataService.Application.Commands;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovMasterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LovMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovMastersQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<LovMasterDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovMasterByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("type/{lovType}")]
    public async Task<ActionResult<IReadOnlyList<LovMasterDto>>> GetByType(string lovType, CancellationToken ct)
        => Ok(await mediator.Send(new GetLovMastersByTypeQuery(lovType), ct));

    [HttpPost]
    public async Task<ActionResult<LovMasterDto>> Create([FromBody] CreateLovMasterDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateLovMasterCommand(dto.LovId, dto.LovType, dto.LovName), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.LovId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<LovMasterDto>> Update(long id, [FromBody] UpdateLovMasterDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new UpdateLovMasterCommand(id, dto.LovType, dto.LovName), ct));

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteLovMasterCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovTypeMasterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LovTypeMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovTypeMastersQuery(), ct));

    [HttpGet("{typeCode}")]
    public async Task<ActionResult<LovTypeMasterDto>> GetById(string typeCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovTypeMasterByIdQuery(typeCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LovTypeMasterDto>> Create([FromBody] CreateLovTypeMasterDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateLovTypeMasterCommand(dto.TypeCode, dto.TypeName), ct);
        return CreatedAtAction(nameof(GetById), new { typeCode = result.TypeCode }, result);
    }

    [HttpPut("{typeCode}")]
    public async Task<ActionResult<LovTypeMasterDto>> Update(string typeCode, [FromBody] UpdateLovTypeMasterDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new UpdateLovTypeMasterCommand(typeCode, dto.TypeName), ct));

    [HttpDelete("{typeCode}")]
    public async Task<IActionResult> Delete(string typeCode, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteLovTypeMasterCommand(typeCode), ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HoldTypeMasterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HoldTypeMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllHoldTypeMastersQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<HoldTypeMasterDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetHoldTypeMasterByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<HoldTypeMasterDto>> Create([FromBody] CreateHoldTypeMasterDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateHoldTypeMasterCommand(dto.HoldId, dto.HoldName, dto.HoldCategory), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.HoldId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<HoldTypeMasterDto>> Update(long id, [FromBody] UpdateHoldTypeMasterDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new UpdateHoldTypeMasterCommand(id, dto.HoldName, dto.HoldCategory), ct));

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteHoldTypeMasterCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationScanParamController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LocationScanParamDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLocationScanParamsQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<LocationScanParamDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLocationScanParamByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LocationScanParamDto>> Create([FromBody] CreateLocationScanParamDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateLocationScanParamCommand(dto.ParamId, dto.LocationId, dto.EffectiveDate, dto.ClosingDate), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ParamId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<LocationScanParamDto>> Update(long id, [FromBody] UpdateLocationScanParamDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new UpdateLocationScanParamCommand(id, dto.EffectiveDate, dto.ClosingDate), ct));

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteLocationScanParamCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScannerMasterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ScannerMasterDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllScannerMastersQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ScannerMasterDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetScannerMasterByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ScannerMasterDto>> Create([FromBody] CreateScannerMasterDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateScannerMasterCommand(dto.DeviceId, dto.DeviceName, dto.DeviceLocationId, dto.DevicePath), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DeviceId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ScannerMasterDto>> Update(long id, [FromBody] UpdateScannerMasterDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new UpdateScannerMasterCommand(id, dto.DeviceName, dto.DeviceLocationId, dto.DevicePath), ct));

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteScannerMasterCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController(MasterDataService.Application.Interfaces.IBlobStorageService blobService) : ControllerBase
{
    [HttpPost("upload/{containerName}")]
    public async Task<IActionResult> Upload(string containerName, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty.");

        using var stream = file.OpenReadStream();
        var url = await blobService.UploadAsync(containerName, file.FileName, stream, file.ContentType, ct);
        return Ok(new { Url = url });
    }

    [HttpGet("download/{containerName}/{fileName}")]
    public async Task<IActionResult> Download(string containerName, string fileName, CancellationToken ct)
    {
        var stream = await blobService.DownloadAsync(containerName, fileName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{containerName}/{fileName}")]
    public async Task<IActionResult> Delete(string containerName, string fileName, CancellationToken ct)
    {
        var result = await blobService.DeleteAsync(containerName, fileName, ct);
        return result ? NoContent() : NotFound();
    }
}
