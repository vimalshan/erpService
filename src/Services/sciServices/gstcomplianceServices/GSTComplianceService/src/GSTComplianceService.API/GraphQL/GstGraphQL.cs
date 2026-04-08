using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using GSTComplianceService.Infrastructure.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GSTComplianceService.API.GraphQL;

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

public class GstMutation
{
    public async Task<long> RegisterGst(
        [Service] MediatR.IMediator mediator,
        string panNo, string? type, string? email, string? mobile, long registeredBy,
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

public class GstMainType : ObjectType<GstMain>
{
    protected override void Configure(IObjectTypeDescriptor<GstMain> descriptor)
    {
        descriptor.Field(f => f.DomainEvents).Ignore();
    }
}

public class GstHsnDetailType : ObjectType<GSTComplianceService.Domain.Entities.GstHsnDetail>
{
    protected override void Configure(IObjectTypeDescriptor<GSTComplianceService.Domain.Entities.GstHsnDetail> descriptor)
    {
        descriptor.Field(f => f.DomainEvents).Ignore();
    }
}

public class GstStateRegDetailType : ObjectType<GSTComplianceService.Domain.Entities.GstStateRegDetail>
{
    protected override void Configure(IObjectTypeDescriptor<GSTComplianceService.Domain.Entities.GstStateRegDetail> descriptor)
    {
        descriptor.Field(f => f.DomainEvents).Ignore();
    }
}

public class GstServiceDetailType : ObjectType<GSTComplianceService.Domain.Entities.GstServiceDetail>
{
    protected override void Configure(IObjectTypeDescriptor<GSTComplianceService.Domain.Entities.GstServiceDetail> descriptor)
    {
        descriptor.Field(f => f.DomainEvents).Ignore();
    }
}

public class GstSupplierType : ObjectType<GSTComplianceService.Domain.Entities.GstSupplier>
{
    protected override void Configure(IObjectTypeDescriptor<GSTComplianceService.Domain.Entities.GstSupplier> descriptor)
    {
        descriptor.Field(f => f.DomainEvents).Ignore();
    }
}
