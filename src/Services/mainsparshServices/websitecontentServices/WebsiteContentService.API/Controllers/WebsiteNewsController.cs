namespace WebsiteContentService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteContentService.Application.Commands.News;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Application.Queries.News;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class WebsiteNewsController(IMediator mediator, ILogger<WebsiteNewsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WebsiteNewsDto>>> GetAllNews(CancellationToken ct)
    {
        var news = await mediator.Send(new GetAllWebsiteNewsQuery(), ct);
        return Ok(news);
    }

    [HttpGet("published")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<WebsiteNewsDto>>> GetPublishedNews(CancellationToken ct)
    {
        var news = await mediator.Send(new GetPublishedWebsiteNewsQuery(), ct);
        return Ok(news);
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<WebsiteNewsDto>>> GetFeaturedNews(CancellationToken ct)
    {
        var news = await mediator.Send(new GetFeaturedWebsiteNewsQuery(), ct);
        return Ok(news);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WebsiteNewsDto>> GetNewsById(long id, CancellationToken ct)
    {
        try
        {
            var news = await mediator.Send(new GetWebsiteNewsByIdQuery(id), ct);
            return Ok(news);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<WebsiteNewsDto>>> GetNewsByCategory(string category, CancellationToken ct)
    {
        var news = await mediator.Send(new GetWebsiteNewsByCategoryQuery(category), ct);
        return Ok(news);
    }

    [HttpPost]
    public async Task<ActionResult<WebsiteNewsDto>> CreateNews(CreateWebsiteNewsCommand command, CancellationToken ct)
    {
        try
        {
            var news = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetNewsById), new { id = news.NewsId }, news);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WebsiteNewsDto>> UpdateNews(long id, UpdateWebsiteNewsCommand command, CancellationToken ct)
    {
        if (id != command.NewsId) return BadRequest(new { message = "ID mismatch." });

        try
        {
            var news = await mediator.Send(command, ct);
            return Ok(news);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/publish")]
    public async Task<ActionResult<WebsiteNewsDto>> PublishNews(long id, PublishWebsiteNewsCommand command, CancellationToken ct)
    {
        if (id != command.NewsId) return BadRequest(new { message = "ID mismatch." });

        try
        {
            var news = await mediator.Send(command, ct);
            return Ok(news);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/archive")]
    public async Task<ActionResult> ArchiveNews(long id, ArchiveWebsiteNewsCommand command, CancellationToken ct)
    {
        if (id != command.NewsId) return BadRequest(new { message = "ID mismatch." });

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

    [HttpPatch("{id}/featured")]
    public async Task<ActionResult> SetFeaturedNews(long id, SetNewsFeaturedCommand command, CancellationToken ct)
    {
        if (id != command.NewsId) return BadRequest(new { message = "ID mismatch." });

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
