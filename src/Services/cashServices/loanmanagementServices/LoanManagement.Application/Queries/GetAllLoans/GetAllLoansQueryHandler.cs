using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Queries.GetAllLoans;

public class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetAllLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<LoanDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = request.OrgId.HasValue
            ? await _loanRepository.GetByOrganizationAsync(request.OrgId.Value, cancellationToken)
            : await _loanRepository.GetAllAsync(cancellationToken);

        return loans.Select(loan => new LoanDto(
            loan.LoanId, loan.LoanKey, loan.LoanOrgId, loan.LoanOrgCurr, loan.LoanCurr,
            loan.LoanDate, loan.LoanTypeId, loan.LoanBankId, loan.LoanAmount, loan.LoanStatus,
            loan.LoanCreatedOn, [], [], []));
    }
}
