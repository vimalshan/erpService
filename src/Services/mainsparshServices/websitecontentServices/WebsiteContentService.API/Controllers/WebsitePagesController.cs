namespace WebsiteContentService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Application.Queries.Pages;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class WebsitePagesController(IMediator mediator, ILogger<WebsitePagesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WebsitePageDto>>> GetAllPages(CancellationToken ct)
    {
        var pages = await mediator.Send(new GetAllWebsitePagesQuery(), ct);
        return Ok(pages);
    }

    [HttpGet("published")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<WebsitePageDto>>> GetPublishedPages(CancellationToken ct)
    {
        var pages = await mediator.Send(new GetPublishedWebsitePagesQuery(), ct);
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WebsitePageDto>> GetPageById(long id, CancellationToken ct)
    {
        try
        {
            var page = await mediator.Send(new GetWebsitePageByIdQuery(id), ct);
            return Ok(page);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("code/{pageCode}")]
    public async Task<ActionResult<WebsitePageDto>> GetPageByCode(string pageCode, CancellationToken ct)
    {
        try
        {
            var page = await mediator.Send(new GetWebsitePageByCodeQuery(pageCode), ct);
            return Ok(page);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<WebsitePageDto>> CreatePage(CreateWebsitePageCommand command, CancellationToken ct)
    {
        try
        {
            var page = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetPageById), new { id = page.PageId }, page);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WebsitePageDto>> UpdatePage(long id, UpdateWebsitePageCommand command, CancellationToken ct)
    {
        if (id != command.PageId) return BadRequest(new { message = "ID mismatch." });

        try
        {
            var page = await mediator.Send(command, ct);
            return Ok(page);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/publish")]
    public async Task<ActionResult<WebsitePageDto>> PublishPage(long id, PublishWebsitePageCommand command, CancellationToken ct)
    {
        if (id != command.PageId) return BadRequest(new { message = "ID mismatch." });

        try
        {
            var page = await mediator.Send(command, ct);
            return Ok(page);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ChangePageStatus(long id, ChangeWebsitePageStatusCommand command, CancellationToken ct)
    {
        if (id != command.PageId) return BadRequest(new { message = "ID mismatch." });

        try
        {
            await mediator.Send(command, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
