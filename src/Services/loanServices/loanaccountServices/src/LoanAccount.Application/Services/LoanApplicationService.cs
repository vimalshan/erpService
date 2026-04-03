using Ardalis.GuardClauses;
using AutoMapper;
using LoanAccount.Application.Commands;
using LoanAccount.Application.DTOs;
using LoanAccount.Application.Queries;
using LoanAccount.Domain.Entities;
using LoanAccount.Domain.Interfaces;
using LoanAccount.Domain.ValueObjects;
using MediatR;

namespace LoanAccount.Application.Services;

/// <summary>
/// Application service for loan operations using CQRS pattern
/// </summary>
public class LoanApplicationService
{
    private readonly ILoanUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoanApplicationService(ILoanUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    /// <summary>
    /// Creates a new loan
    /// </summary>
    public async Task<long> CreateLoanAsync(CreateLoanCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var disbursementType = DisbursementType.Create(command.DisbursementType);

        var loan = LoanMain.Create(
            loanNo: GenerateLoanNumber(),
            loanAppId: command.LoanAppId,
            empSysId: command.EmployeeId,
            loanId: command.LoanId,
            gradeId: command.GradeId,
            principalAmount: command.PrincipalAmount,
            disbursementType: disbursementType,
            loanDate: command.LoanDate,
            firstInstallmentDate: command.FirstInstallmentDate,
            unitId: command.UnitId,
            subClassId: command.SubClassId,
            reason: command.Reason,
            guarantorId: command.GuarantorId,
            createdBy: command.CreatedBy);

        await _unitOfWork.LoanMainRepository.AddAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return loan.LoanNo;
    }

    /// <summary>
    /// Approves a loan and sets interest rate
    /// </summary>
    public async Task<bool> ApproveLoanAsync(ApproveLoanCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(command.LoanNo, cancellationToken);
        if (loan is null)
            throw new InvalidOperationException($"Loan {command.LoanNo} not found");

        var interestRate = new InterestRate(command.InterestRate);
        loan.Approve(interestRate, command.ApprovedBy, command.ApprovalRemarks);

        _unitOfWork.LoanMainRepository.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Disburses a loan amount
    /// </summary>
    public async Task<bool> DisburseLoanAsync(DisburseLoanCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(command.LoanNo, cancellationToken);
        if (loan is null)
            throw new InvalidOperationException($"Loan {command.LoanNo} not found");

        loan.Disburse(command.Amount, command.DisbursedBy);

        _unitOfWork.LoanMainRepository.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Creates EMI installments for a loan
    /// </summary>
    public async Task<bool> CreateInstallmentsAsync(CreateLoanInstallmentsCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(command.LoanNo, cancellationToken);
        if (loan is null)
            throw new InvalidOperationException($"Loan {command.LoanNo} not found");

        for (int i = 1; i <= command.NumberOfInstallments; i++)
        {
            var installmentDate = command.FirstInstallmentDate.AddMonths(i - 1);
            var principalOutstanding = Math.Max(0, command.EMIAmount * (command.NumberOfInstallments - i));

            var installment = LoanInstallment.Create(
                loanNo: command.LoanNo,
                unitId: loan.UnitId,
                installmentNo: i,
                installmentAmount: command.EMIAmount,
                principalOutstanding: principalOutstanding,
                interestRatePercentage: 0,
                installmentDate: installmentDate,
                createdBy: command.CreatedBy);

            loan.RecordInstallment(installment);
            await _unitOfWork.InstallmentRepository.AddAsync(installment, cancellationToken);
        }

        _unitOfWork.LoanMainRepository.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Records an EMI payment
    /// </summary>
    public async Task<bool> RecordEMIPaymentAsync(RecordEMIPaymentCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var installment = await _unitOfWork.InstallmentRepository.GetByInstallmentIdAsync(command.InstallmentId, cancellationToken);
        if (installment is null)
            throw new InvalidOperationException($"Installment {command.InstallmentId} not found");

        installment.RecordPayment(command.PrincipalPaid, command.InterestPaid, command.PaidBy);

        _unitOfWork.InstallmentRepository.Update(installment);

        // Record ledger entry
        var ledgerEntry = LoanLedger.Create(
            loanNo: command.LoanNo,
            empSysId: 0, // Would be obtained from context
            empNo: 0,
            unitId: 0,
            dcFlag: 'D',
            description: "EMI Payment",
            amount: command.PrincipalPaid + command.InterestPaid,
            transactionType: "PAYMENT",
            referenceNo: command.InstallmentId,
            scheduleId: command.InstallmentId,
            createdBy: command.PaidBy);

        await _unitOfWork.LedgerRepository.AddAsync(ledgerEntry, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Settles a loan
    /// </summary>
    public async Task<bool> SettleLoanAsync(SettleLoanCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(command.LoanNo, cancellationToken);
        if (loan is null)
            throw new InvalidOperationException($"Loan {command.LoanNo} not found");

        loan.Settle(command.SettledBy);

        _unitOfWork.LoanMainRepository.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Closes a loan
    /// </summary>
    public async Task<bool> CloseLoanAsync(CloseLoanCommand command, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(command, nameof(command));

        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(command.LoanNo, cancellationToken);
        if (loan is null)
            throw new InvalidOperationException($"Loan {command.LoanNo} not found");

        loan.Close(DateTime.UtcNow, command.Reason);

        _unitOfWork.LoanMainRepository.Update(loan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Gets loan details with related data
    /// </summary>
    public async Task<LoanDetailsResponse?> GetLoanDetailsAsync(GetLoanDetailsQuery query, CancellationToken cancellationToken = default)
    {
        var loan = await _unitOfWork.LoanMainRepository.GetByLoanNumberAsync(query.LoanNo, cancellationToken);
        if (loan is null)
            return null;

        var installments = await _unitOfWork.InstallmentRepository.GetByLoanNoAsync(loan.LoanNo, cancellationToken);
        var ledgerEntries = await _unitOfWork.LedgerRepository.GetByLoanNoAsync(loan.LoanNo, cancellationToken);

        return new LoanDetailsResponse
        {
            LoanNo = loan.LoanNo,
            EmployeeId = loan.EmpSysId,
            PrincipalAmount = loan.PrincipalAmount.Amount,
            DisbursedAmount = loan.DisbursedAmount.Amount,
            OutstandingAmount = loan.PrincipalOutstanding.Amount,
            Status = loan.LoanStatus.Status,
            LoanDate = loan.LoanDate,
            ClosureDate = loan.LoanClosureDate,
            Installments = _mapper.Map<IEnumerable<InstallmentResponse>>(installments),
            LedgerEntries = _mapper.Map<IEnumerable<LoanLedgerEntryResponse>>(ledgerEntries)
        };
    }

    private static long GenerateLoanNumber()
    {
        // In production, this would generate a sequence number from database or ID generator
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
