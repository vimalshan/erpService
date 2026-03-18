using AutoMapper;
using LoanService.Application.Common;
using LoanService.Application.DTOs;
using LoanService.Domain.Entities;
using LoanService.Domain.Interfaces;
using MediatR;

namespace LoanService.Application.Loans.Commands;

public class CreateLoanHandler : IRequestHandler<CreateLoanCommand, Result<LoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateLoanHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<LoanDto>> Handle(CreateLoanCommand req, CancellationToken ct)
    {
        if (await _uow.Loans.ExistsAsync(req.LoanNo, ct))
            return Result<LoanDto>.Failure($"Loan {req.LoanNo} already exists.");

        var loan = LoanMain.Create(req.LoanNo, req.MemberId, req.LoanAmount, req.LoanType, req.LoanReason, req.CreatedBy);

        if (!string.IsNullOrEmpty(req.TrustCode)) loan.SetTrustCode(req.TrustCode);
        if (req.Rate.HasValue) loan.SetRate(req.Rate.Value);
        if (!string.IsNullOrEmpty(req.Tenure)) loan.SetTenure(req.Tenure);

        await _uow.Loans.AddAsync(loan, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<LoanDto>.Success(_mapper.Map<LoanDto>(loan));
    }
}

public class ApproveLoanHandler : IRequestHandler<ApproveLoanCommand, Result<LoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ApproveLoanHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<LoanDto>> Handle(ApproveLoanCommand req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        if (loan is null) return Result<LoanDto>.Failure("Loan not found.");

        loan.Approve(req.ApprovalDate);
        await _uow.SaveChangesAsync(ct);

        return Result<LoanDto>.Success(_mapper.Map<LoanDto>(loan));
    }
}

public class CloseLoanHandler : IRequestHandler<CloseLoanCommand, Result<LoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CloseLoanHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<LoanDto>> Handle(CloseLoanCommand req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        if (loan is null) return Result<LoanDto>.Failure("Loan not found.");

        loan.Close(req.ClosureDate);
        await _uow.SaveChangesAsync(ct);

        return Result<LoanDto>.Success(_mapper.Map<LoanDto>(loan));
    }
}

public class AddRepaymentHandler : IRequestHandler<AddRepaymentCommand, Result<RepaymentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddRepaymentHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<RepaymentDto>> Handle(AddRepaymentCommand req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        if (loan is null) return Result<RepaymentDto>.Failure("Loan not found.");

        var repayment = new LoanRepayment(req.LoanNo, req.InstallmentNo, req.Amount, req.DueDate);
        loan.AddRepayment(repayment);
        await _uow.SaveChangesAsync(ct);

        return Result<RepaymentDto>.Success(_mapper.Map<RepaymentDto>(repayment));
    }
}

public class MakePaymentHandler : IRequestHandler<MakePaymentCommand, Result<RepaymentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MakePaymentHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<RepaymentDto>> Handle(MakePaymentCommand req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        if (loan is null) return Result<RepaymentDto>.Failure("Loan not found.");

        var repayment = loan.Repayments.FirstOrDefault(r => r.RepayId == req.RepaymentId);
        if (repayment is null) return Result<RepaymentDto>.Failure("Repayment not found.");

        repayment.MarkPaid(req.PaidAmount, req.PaidDate);
        loan.AddDomainEvent(new Domain.Events.RepaymentMadeEvent(req.LoanNo, req.RepaymentId, req.PaidAmount));
        await _uow.SaveChangesAsync(ct);

        return Result<RepaymentDto>.Success(_mapper.Map<RepaymentDto>(repayment));
    }
}

public class AddDeductionHandler : IRequestHandler<AddDeductionCommand, Result<DeductionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddDeductionHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<DeductionDto>> Handle(AddDeductionCommand req, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(req.LoanNo, ct);
        if (loan is null) return Result<DeductionDto>.Failure("Loan not found.");

        var deduction = new LoanDeduction(req.LoanNo, req.Amount, req.Date, req.ContributionId);
        loan.AddDeduction(deduction);
        await _uow.SaveChangesAsync(ct);

        return Result<DeductionDto>.Success(_mapper.Map<DeductionDto>(deduction));
    }
}
