using MediatR;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Data.ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public ITrainingDetailRepository TrainingDetails { get; }
    public IInstituteMasterRepository Institutes { get; }
    public IProgramLovRepository ProgramLovs { get; }

    public UnitOfWork(
        Data.ApplicationDbContext context,
        IMediator mediator,
        ITrainingDetailRepository trainingDetails,
        IInstituteMasterRepository institutes,
        IProgramLovRepository programLovs)
    {
        _context = context;
        _mediator = mediator;
        TrainingDetails = trainingDetails;
        Institutes = institutes;
        ProgramLovs = programLovs;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving
        var domainEvents = _context.GetPendingDomainEvents().ToList();

        var result = await _context.SaveChangesAsync(cancellationToken);

        // Dispatch domain events after successful save
        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }

    public void Dispose() => _context.Dispose();
}
