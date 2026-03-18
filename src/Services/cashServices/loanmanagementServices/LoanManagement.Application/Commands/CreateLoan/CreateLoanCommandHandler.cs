using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Commands.CreateLoan;

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoanDto> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _loanRepository.GetNextIdAsync(cancellationToken);

        var loan = LoanMain.Create(
            nextId,
            request.LoanKey,
            request.OrgId,
            request.LoanAmount,
            request.LoanTypeId,
            request.BankId,
            request.CreatedBy,
            request.LoanDate,
            request.OrgCurr,
            request.LoanCurr);

        await _loanRepository.AddAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoanDto(
            loan.LoanId, loan.LoanKey, loan.LoanOrgId, loan.LoanOrgCurr, loan.LoanCurr,
            loan.LoanDate, loan.LoanTypeId, loan.LoanBankId, loan.LoanAmount, loan.LoanStatus,
            loan.LoanCreatedOn, [], [], []);
    }
}
