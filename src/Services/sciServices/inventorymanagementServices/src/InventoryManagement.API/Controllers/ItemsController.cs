using InventoryManagement.Application.Commands.Items;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Queries.Items;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobStorageService _blobService;
    private readonly BlobStorageOptions _blobOptions;

    public ItemsController(IMediator mediator, IBlobStorageService blobService, IOptions<BlobStorageOptions> blobOptions)
    {
        _mediator = mediator;
        _blobService = blobService;
        _blobOptions = blobOptions.Value;
    }

    /// <summary>Get all items.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllItemsQuery(), ct));

    /// <summary>Get item by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetItemByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get items by Oracle code.</summary>
    [HttpGet("oracle/{code}")]
    [ProducesResponseType(typeof(ItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByOracleCode(string code, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetItemByOracleCodeQuery(code), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get items by product ID.</summary>
    [HttpGet("product/{productId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
    public async Task<IActionResult> GetByProduct(int productId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetItemsByProductQuery(productId), ct));

    /// <summary>Register a new item.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterItemCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SciItemId }, result);
    }

    /// <summary>Update an item.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemCommand command, CancellationToken ct)
    {
        if (id != command.SciItemId) return BadRequest("Route ID doesn't match command ID.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Upload item image to blob storage.</summary>
    [HttpPost("{id:int}/image")]
    [ProducesResponseType(typeof(UploadItemImageResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest("No file uploaded.");

        var blobName = $"items/{id}/{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobService.UploadAsync(_blobOptions.ItemImagesContainer, blobName, stream, file.ContentType, ct);

        return Ok(new UploadItemImageResponse(id, url));
    }
}
