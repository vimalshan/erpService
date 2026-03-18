using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using GSTComplianceService.Infrastructure.Persistence;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

namespace GSTComplianceService.API.GraphQL;

[QueryType]
public class GstQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<GstMain> GetGstRegistrations([Service] GstDbContext context)
        => context.GstMains.Include(g => g.HsnDetails).Include(g => g.StateRegDetails);

    public async Task<GstMain?> GetGstById([Service] IGstMainRepository repo, long id, CancellationToken ct)
        => await repo.GetByIdAsync(id, ct);

    public async Task<GstMain?> GetGstByPan([Service] IGstMainRepository repo, string panNo, CancellationToken ct)
        => await repo.GetByPanNoAsync(panNo, ct);
}

[MutationType]
public class GstMutation
{
    public async Task<long> RegisterGst(
        [Service] MediatR.IMediator mediator,
        string panNo, char? type, string? email, long? mobile, long registeredBy,
        CancellationToken ct)
    {
        return await mediator.Send(
            new Application.Features.GstMain.Commands.RegisterGstCommand(panNo, type, email, mobile, registeredBy), ct);
    }

    public async Task<bool> ActivateGst([Service] MediatR.IMediator mediator, long gstId, CancellationToken ct)
    {
        await mediator.Send(new Application.Features.GstMain.Commands.ActivateGstCommand(gstId), ct);
        return true;
    }
}
