using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;

namespace BankService.Infrastructure.Repositories;

public class UnitOfWork(BankDbContext context,
    IBankMasterRepository bankMasters,
    IBankAccountRepository bankAccounts,
    IChequeDetailRepository chequeDetails,
    IChequeRegisterRepository chequeRegisters,
    IPaymentReconciliationRepository paymentReconciliations) : IUnitOfWork
{
    public IBankMasterRepository BankMasters => bankMasters;
    public IBankAccountRepository BankAccounts => bankAccounts;
    public IChequeDetailRepository ChequeDetails => chequeDetails;
    public IChequeRegisterRepository ChequeRegisters => chequeRegisters;
    public IPaymentReconciliationRepository PaymentReconciliations => paymentReconciliations;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
