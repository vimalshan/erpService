using EximManagement.Application.Commands.DataFiles;
using EximManagement.Application.Commands.Products;
using EximManagement.Application.DTOs;
using EximManagement.Application.Queries.DataFiles;
using EximManagement.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EximManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EximDataFilesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? fileType, CancellationToken ct)
    {
        var files = await mediator.Send(new GetAllDataFilesQuery(fileType), ct);
        return Ok(files);
    }

    [HttpGet("{fileId:long}")]
    public async Task<IActionResult> GetById(long fileId, CancellationToken ct)
    {
        var file = await mediator.Send(new GetDataFileByIdQuery(fileId), ct);
        return file is null ? NotFound() : Ok(file);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEximDataFileDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateDataFileCommand(
            dto.FileId, dto.FileType, dto.FileName, dto.UploadedBy,
            dto.FileSource, dto.Remarks, dto.DataTypeCode, dto.DataTypeMonth, dto.DataXml), ct);
        return CreatedAtAction(nameof(GetById), new { fileId = result.FileId }, result);
    }

    [HttpDelete("{fileId:long}")]
    public async Task<IActionResult> Delete(long fileId, [FromQuery] string deletedBy, CancellationToken ct)
    {
        await mediator.Send(new DeleteDataFileCommand(fileId, deletedBy), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EximProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllProductsQuery(), ct));

    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetById(long productId, CancellationToken ct)
    {
        var product = await mediator.Send(new GetProductByIdQuery(productId), ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEximProductDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateProductCommand(
            dto.ProductName, dto.OracleCode, dto.UpdatedBy), ct);
        return CreatedAtAction(nameof(GetById), new { productId = result.ProductId }, result);
    }

    [HttpPut("{productId:long}")]
    public async Task<IActionResult> Update(long productId, [FromBody] CreateEximProductDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateProductCommand(
            productId, dto.ProductName, dto.OracleCode, dto.UpdatedBy), ct);
        return Ok(result);
    }

    [HttpDelete("{productId:long}")]
    public async Task<IActionResult> Deactivate(long productId, [FromQuery] long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new DeactivateProductCommand(productId, updatedBy), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/exim")]
[Authorize]
public class EximDataController(IMediator mediator) : ControllerBase
{
    [HttpGet("export")]
    public async Task<IActionResult> GetExportData([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await mediator.Send(new GetExportDataByDateRangeQuery(from, to), ct));

    [HttpGet("import")]
    public async Task<IActionResult> GetImportData([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await mediator.Send(new GetImportDataByDateRangeQuery(from, to), ct));
}
