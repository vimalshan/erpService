using MediatR;
using Microsoft.Extensions.Logging;
using LoanTransaction.Application.Commands;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.Entities;
using LoanTransaction.Domain.Interfaces;
using LoanTransaction.Domain.ValueObjects;
using AutoMapper;

namespace LoanTransaction.Application.CommandHandlers;

public class DisburseLoanCommandHandler : IRequestHandler<DisburseLoanCommand, long>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmiCalculatorService _emiCalc;
    private readonly IPublisher _publisher;
    private readonly ILogger<DisburseLoanCommandHandler> _logger;

    public DisburseLoanCommandHandler(
        IUnitOfWork uow,
        IEmiCalculatorService emiCalc,
        IPublisher publisher,
        ILogger<DisburseLoanCommandHandler> logger)
    {
        _uow = uow;
        _emiCalc = emiCalc;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<long> Handle(DisburseLoanCommand cmd, CancellationToken ct)
    {
        await _uow.BeginTransactionAsync(ct);
        try
        {
            var disbType = DisbursementType.FromValue(cmd.DisbursementType);
            var principal = Money.Create(cmd.PrincipalAmount);
            var recoveryMethod = RecoveryMethod.FromValue(cmd.RecoveryMethod);
            var lastInstDate = cmd.FirstInstallmentDate.AddMonths(cmd.TenureMonths - 1);

            var loan = LoanAggregate.Disburse(
                cmd.ApplicationId, cmd.EmployeeId, cmd.LoanDefinitionId,
                cmd.GradeId, cmd.UnitId, cmd.SubclassId, cmd.GuarantorId,
                disbType, principal, principal, recoveryMethod,
                cmd.EffectiveDate, cmd.FirstInstallmentDate, lastInstDate,
                cmd.Reason, cmd.CompoundingFactor[0], cmd.InterestFrequency[0],
                cmd.HasEmployeeInterestRate,
                cmd.AmountEdId, cmd.PrnEdId, cmd.IntEdId, cmd.CreatedBy);

            await _uow.Loans.AddAsync(loan, ct);
            await _uow.SaveChangesAsync(ct);

            // Generate EMI schedule
            var schedule = _emiCalc.GenerateSchedule(
                cmd.PrincipalAmount, cmd.InterestRate, cmd.TenureMonths, cmd.FirstInstallmentDate).ToList();

            decimal outstanding = cmd.PrincipalAmount;
            foreach (var item in schedule)
            {
                var installment = new LoanInstallment
                {
                    LoanNo = loan.Id,
                    UnitId = cmd.UnitId,
                    InstallmentDate = item.InstallmentDate,
                    InstallmentNo = item.InstallmentNo,
                    InstallmentAmount = item.InstallmentAmount,
                    PrincipalOutstanding = item.PrincipalOutstanding,
                    InterestRate = cmd.InterestRate,
                    Remarks = $"EMI {item.InstallmentNo}",
                    UpdatedBy = cmd.CreatedBy,
                    UpdatedOn = DateTime.UtcNow
                };
                await _uow.Installments.AddRangeAsync(new[] { installment }, ct);
            }

            // Set employee interest rate
            loan.SetEmployeeInterestRate(cmd.InterestRate,
                schedule.FirstOrDefault()?.InstallmentAmount ?? 0,
                cmd.TenureMonths, cmd.CreatedBy);

            // Add opening ledger entry (credit disbursement)
            await _uow.LedgerEntries.AddAsync(new LoanLedger
            {
                LoanNo = loan.Id,
                EmployeeId = cmd.EmployeeId,
                UnitId = cmd.UnitId,
                EmployeeNo = cmd.EmployeeId,
                TransactionDate = cmd.EffectiveDate,
                DCFlag = 'C',
                Description = "Loan Disbursement",
                TransactionAmount = cmd.PrincipalAmount,
                TransactionType = "DIS",
                UpdatedBy = cmd.CreatedBy,
                UpdatedOn = DateTime.UtcNow
            }, ct);

            await _uow.CommitAsync(ct);

            foreach (var ev in loan.GetDomainEvents())
                await _publisher.Publish(ev, ct);

            _logger.LogInformation("Loan {LoanNo} disbursed for Employee {EmpId} Amount {Amount}",
                loan.Id, cmd.EmployeeId, cmd.PrincipalAmount);

            return loan.Id;
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}

public class RecordEmiPaymentCommandHandler : IRequestHandler<RecordEmiPaymentCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IPublisher _publisher;
    private readonly ILogger<RecordEmiPaymentCommandHandler> _logger;

    public RecordEmiPaymentCommandHandler(IUnitOfWork uow, IPublisher publisher, ILogger<RecordEmiPaymentCommandHandler> logger)
    {
        _uow = uow;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<bool> Handle(RecordEmiPaymentCommand cmd, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdWithInstallmentsAsync(cmd.LoanNo, ct)
            ?? throw new KeyNotFoundException($"Loan {cmd.LoanNo} not found.");

        loan.RecordEmiPayment(cmd.InstallmentId, cmd.PrincipalPaid, cmd.InterestPaid, cmd.PaidBy);

        await _uow.Loans.UpdateAsync(loan, ct);
        await _uow.SaveChangesAsync(ct);

        foreach (var ev in loan.GetDomainEvents())
            await _publisher.Publish(ev, ct);

        _logger.LogInformation("EMI payment recorded for Loan {LoanNo} Installment {InstId}", cmd.LoanNo, cmd.InstallmentId);
        return true;
    }
}

public class CloseLoanCommandHandler : IRequestHandler<CloseLoanCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IPublisher _publisher;

    public CloseLoanCommandHandler(IUnitOfWork uow, IPublisher publisher)
    {
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<bool> Handle(CloseLoanCommand cmd, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(cmd.LoanNo, ct)
            ?? throw new KeyNotFoundException($"Loan {cmd.LoanNo} not found.");

        loan.CloseLoan(cmd.ClosedBy, cmd.ClosureType);

        await _uow.Loans.UpdateAsync(loan, ct);
        await _uow.SaveChangesAsync(ct);

        foreach (var ev in loan.GetDomainEvents())
            await _publisher.Publish(ev, ct);

        return true;
    }
}

public class AdjustLoanCommandHandler : IRequestHandler<AdjustLoanCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public AdjustLoanCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(AdjustLoanCommand cmd, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(cmd.LoanNo, ct)
            ?? throw new KeyNotFoundException($"Loan {cmd.LoanNo} not found.");

        loan.AddAdjustment(cmd.AdjLoanNo, cmd.AdjPrincipalAmount, cmd.AdjInterestAmount, cmd.UpdatedBy);

        await _uow.Loans.UpdateAsync(loan, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}

public class SetEmployeeInterestRateCommandHandler : IRequestHandler<SetEmployeeInterestRateCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public SetEmployeeInterestRateCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(SetEmployeeInterestRateCommand cmd, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(cmd.LoanNo, ct)
            ?? throw new KeyNotFoundException($"Loan {cmd.LoanNo} not found.");

        loan.SetEmployeeInterestRate(cmd.Rate, cmd.EmiAmount, cmd.NumberOfInstallments, cmd.ModifiedBy);

        await _uow.Loans.UpdateAsync(loan, ct);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateEmiScheduleCommandHandler : IRequestHandler<CreateEmiScheduleCommand, IEnumerable<EmiScheduleItemDto>>
{
    private readonly IEmiCalculatorService _emiCalc;
    private readonly IMapper _mapper;

    public CreateEmiScheduleCommandHandler(IEmiCalculatorService emiCalc, IMapper mapper)
    {
        _emiCalc = emiCalc;
        _mapper = mapper;
    }

    public Task<IEnumerable<EmiScheduleItemDto>> Handle(CreateEmiScheduleCommand cmd, CancellationToken ct)
    {
        var schedule = _emiCalc.GenerateSchedule(cmd.PrincipalAmount, cmd.InterestRate, cmd.TenureMonths, cmd.FirstInstallmentDate);
        return Task.FromResult(_mapper.Map<IEnumerable<EmiScheduleItemDto>>(schedule));
    }
}
