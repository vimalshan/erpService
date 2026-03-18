namespace LoanService.Domain.Interfaces;

public interface IUnitOfWork
{
    ILoanRepository Loans { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
