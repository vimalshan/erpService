using MediatR;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Commands.CloseLoan;

public class CloseLoanCommandHandler : IRequestHandler<CloseLoanCommand, bool>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CloseLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new LoanDomainException($"Loan {request.LoanId} not found.");

        loan.CloseLoan(request.ModifiedBy);
        await _loanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
