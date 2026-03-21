using ComplaintService.Application.DTOs;
using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintGroupsController(IComplaintGroupRepository groupRepo, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<ComplaintGroupDto>>(200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var groups = await groupRepo.GetAllAsync(ct);
        var dtos = groups.Select(g => new ComplaintGroupDto(
            g.UnitCode, g.GroupId, g.GroupName, g.GroupDesc, g.GroupSrc,
            g.RegPin, g.Shift, g.Mail, g.RegDate));
        return Ok(dtos);
    }

    [HttpGet("{groupId}")]
    [ProducesResponseType<ComplaintGroupDto>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(string groupId, CancellationToken ct)
    {
        var group = await groupRepo.GetByIdAsync(groupId, ct);
        if (group is null) return NotFound();
        return Ok(new ComplaintGroupDto(
            group.UnitCode, group.GroupId, group.GroupName, group.GroupDesc, group.GroupSrc,
            group.RegPin, group.Shift, group.Mail, group.RegDate));
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateComplaintGroupRequest request, CancellationToken ct)
    {
        var group = ComplaintGroup.Create(request.UnitCode, request.GroupId, request.GroupName,
            request.GroupSrc, request.RegPin, request.Shift, request.Mail);

        await groupRepo.AddAsync(group, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { groupId = group.GroupId }, group.GroupId);
    }
}
