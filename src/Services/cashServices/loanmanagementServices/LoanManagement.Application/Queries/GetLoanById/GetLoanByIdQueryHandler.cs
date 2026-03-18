using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Queries.GetLoanById;

public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto?>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoanByIdQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken);
        if (loan is null) return null;

        return new LoanDto(
            loan.LoanId, loan.LoanKey, loan.LoanOrgId, loan.LoanOrgCurr, loan.LoanCurr,
            loan.LoanDate, loan.LoanTypeId, loan.LoanBankId, loan.LoanAmount, loan.LoanStatus,
            loan.LoanCreatedOn,
            loan.Disbursements.Select(d => new DisbursementScheduleDto(
                d.DisbId, d.DisbLoanId, d.DisbDate, d.DisbAmount, d.DisbExcRate, d.DisbExcAmt)).ToList(),
            loan.Interests.Select(i => new InterestDto(
                i.IntId, i.IntLoanId, i.IntRateType, i.IntPer, i.IntFloatTypeId, i.IntEffDate, i.IntClsDate)).ToList(),
            loan.Repayments.Select(r => new RepaymentScheduleDto(
                r.RepayId, r.RepayLoanId, r.RepayDate, r.RepayAmt, r.RepayFlag)).ToList()
        );
    }
}
