namespace TrainingDevelopment.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITrainingDetailRepository TrainingDetails { get; }
    IInstituteMasterRepository Institutes { get; }
    IProgramLovRepository ProgramLovs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
