using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.OrgParams.Commands;
using OrganizationSetup.Application.OrgParams.Queries;

namespace OrganizationSetup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrgParamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrgParamsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("org/{orgId}")]
    public async Task<ActionResult<IEnumerable<OrgParamsDto>>> GetByOrg(long orgId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetOrgParamsByOrgQuery(orgId), ct));

    [HttpGet("org/{orgId}/type/{paramType}")]
    public async Task<ActionResult<OrgParamsDto>> GetByType(long orgId, string paramType, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetOrgParamByTypeQuery(orgId, paramType), ct));

    [HttpPost]
    public async Task<ActionResult<OrgParamsDto>> Create([FromBody] CreateOrgParamCommand command, CancellationToken ct) =>
        Ok(await _mediator.Send(command, ct));

    [HttpPut]
    public async Task<ActionResult<OrgParamsDto>> Update([FromBody] UpdateOrgParamCommand command, CancellationToken ct) =>
        Ok(await _mediator.Send(command, ct));
}
