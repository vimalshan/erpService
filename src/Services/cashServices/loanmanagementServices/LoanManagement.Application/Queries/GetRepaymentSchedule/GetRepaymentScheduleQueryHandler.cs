using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Queries.GetRepaymentSchedule;

public class GetRepaymentScheduleQueryHandler
    : IRequestHandler<GetRepaymentScheduleQuery, IEnumerable<RepaymentScheduleDto>>
{
    private readonly IRepaymentRepository _repaymentRepository;

    public GetRepaymentScheduleQueryHandler(IRepaymentRepository repaymentRepository)
    {
        _repaymentRepository = repaymentRepository;
    }

    public async Task<IEnumerable<RepaymentScheduleDto>> Handle(
        GetRepaymentScheduleQuery request,
        CancellationToken cancellationToken)
    {
        var repayments = await _repaymentRepository.GetByLoanIdAsync(request.LoanId, cancellationToken);
        return repayments.Select(r => new RepaymentScheduleDto(
            r.RepayId, r.RepayLoanId, r.RepayDate, r.RepayAmt, r.RepayFlag));
    }
}
