namespace FaqServices.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFaqGradeRepository FaqGrades { get; }
    IFaqQuestionRepository FaqQuestions { get; }
    IFaqAnswerRepository FaqAnswers { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
