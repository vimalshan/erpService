using MediatR;
using ProjectService.Domain.Common;
using ProjectService.Domain.Interfaces;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class UnitOfWork(ProjectDbContext context, IMediator mediator) : IUnitOfWork
{
    private IProjectMainRepository? _projectMains;
    private IProjectMasterRepository? _projectMasters;
    private IProjectTypeMasterRepository? _projectTypes;
    private IProjectMemberRepository? _projectMembers;

    public IProjectMainRepository ProjectMains =>
        _projectMains ??= new ProjectMainRepository(context);

    public IProjectMasterRepository ProjectMasters =>
        _projectMasters ??= new ProjectMasterRepository(context);

    public IProjectTypeMasterRepository ProjectTypes =>
        _projectTypes ??= new ProjectTypeMasterRepository(context);

    public IProjectMemberRepository ProjectMembers =>
        _projectMembers ??= new ProjectMemberRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
