using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace EmailNotification.API.Controllers;

/// <summary>
/// Base API controller with common functionality
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the BaseApiController class
    /// </summary>
    /// <param name="mediator">The MediatR mediator</param>
    protected BaseApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}
