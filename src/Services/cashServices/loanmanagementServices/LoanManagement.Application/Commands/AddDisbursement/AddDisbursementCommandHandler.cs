using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Commands.AddDisbursement;

public class AddDisbursementCommandHandler : IRequestHandler<AddDisbursementCommand, DisbursementScheduleDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDisbursementCommandHandler(
        ILoanRepository loanRepository,
        IDisbursementRepository disbursementRepository,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _disbursementRepository = disbursementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DisbursementScheduleDto> Handle(AddDisbursementCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new LoanDomainException($"Loan {request.LoanId} not found.");

        var disbursement = LoanDisbursementSchedule.Create(
            0,
            request.LoanId,
            request.DisbDate,
            request.Amount,
            request.ExcRate);

        loan.AddDisbursement(disbursement);
        await _disbursementRepository.AddAsync(disbursement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DisbursementScheduleDto(
            disbursement.DisbId, disbursement.DisbLoanId, disbursement.DisbDate,
            disbursement.DisbAmount, disbursement.DisbExcRate, disbursement.DisbExcAmt);
    }
}
