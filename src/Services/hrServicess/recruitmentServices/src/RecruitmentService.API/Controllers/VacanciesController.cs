using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentService.Application.Commands.Vacancies;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Interfaces;
using RecruitmentService.Application.Queries.Vacancies;

namespace RecruitmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VacanciesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobStorageService _blobStorage;

    public VacanciesController(IMediator mediator, IBlobStorageService blobStorage)
    {
        _mediator = mediator;
        _blobStorage = blobStorage;
    }

    /// <summary>Get all open vacancies.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VacancySummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllVacanciesQuery(), ct));

    /// <summary>Get vacancy by ID.</summary>
    [HttpGet("{id:decimal}")]
    [ProducesResponseType(typeof(VacancyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetVacancyByIdQuery(id), ct));

    /// <summary>Get vacancies by unit.</summary>
    [HttpGet("unit/{unit}")]
    [ProducesResponseType(typeof(IEnumerable<VacancySummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUnit(string unit, CancellationToken ct)
        => Ok(await _mediator.Send(new GetVacanciesByUnitQuery(unit), ct));

    /// <summary>Create a new vacancy. Requires HR role.</summary>
    [HttpPost]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVacancyRequest request, CancellationToken ct)
    {
        var postedBy = GetCurrentUserId();
        var id = await _mediator.Send(new CreateVacancyCommand(request, postedBy), ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Update vacancy details. Requires HR role.</summary>
    [HttpPut("{id:decimal}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateVacancyRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateVacancyCommand(id, request, GetCurrentUserId()), ct);
        return NoContent();
    }

    /// <summary>Close a vacancy. Requires HR role.</summary>
    [HttpPost("{id:decimal}/close")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(decimal id, CancellationToken ct)
    {
        await _mediator.Send(new CloseVacancyCommand(id, GetCurrentUserId()), ct);
        return NoContent();
    }

    /// <summary>Upload vacancy attachment to Azure Blob Storage.</summary>
    [HttpPost("{id:decimal}/attachment")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadAttachment(decimal id, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty.");

        var fileName = $"vacancy-{id}-{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        await using var stream = file.OpenReadStream();
        var uri = await _blobStorage.UploadAsync(stream, fileName, "vacancy-attachments", ct);

        await _mediator.Send(new UpdateVacancyAttachmentCommand(id, fileName), ct);
        return Ok(new { uri });
    }

    private decimal GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return decimal.TryParse(claim, out var id) ? id : 0;
    }
}
