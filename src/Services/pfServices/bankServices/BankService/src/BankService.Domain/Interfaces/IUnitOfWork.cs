namespace BankService.Domain.Interfaces;

public interface IUnitOfWork
{
    IBankMasterRepository BankMasters { get; }
    IBankAccountRepository BankAccounts { get; }
    IChequeDetailRepository ChequeDetails { get; }
    IChequeRegisterRepository ChequeRegisters { get; }
    IPaymentReconciliationRepository PaymentReconciliations { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
