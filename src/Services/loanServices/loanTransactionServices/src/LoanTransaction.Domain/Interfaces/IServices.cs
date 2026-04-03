using LoanTransaction.Domain.Interfaces;

namespace LoanTransaction.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ILoanRepository Loans { get; }
    ILoanInstallmentRepository Installments { get; }
    ILoanSettlementRepository Settlements { get; }
    ILoanLedgerRepository LedgerEntries { get; }

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IEmiCalculatorService
{
    decimal CalculateEmi(decimal principalAmount, int ratePerAnnum, int tenureMonths);
    IEnumerable<EmiScheduleItem> GenerateSchedule(decimal principalAmount, int ratePerAnnum, int tenureMonths, DateTime firstInstallmentDate);
}

public class EmiScheduleItem
{
    public int InstallmentNo { get; set; }
    public DateTime InstallmentDate { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal PrincipalComponent { get; set; }
    public decimal InterestComponent { get; set; }
    public decimal PrincipalOutstanding { get; set; }
}

public interface IMessageBus : IAsyncDisposable
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class;
}
