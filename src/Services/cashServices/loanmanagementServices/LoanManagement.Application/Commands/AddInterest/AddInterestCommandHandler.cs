using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Commands.AddInterest;

public class AddInterestCommandHandler : IRequestHandler<AddInterestCommand, InterestDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IInterestRepository _interestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddInterestCommandHandler(
        ILoanRepository loanRepository,
        IInterestRepository interestRepository,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _interestRepository = interestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InterestDto> Handle(AddInterestCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new LoanDomainException($"Loan {request.LoanId} not found.");

        var rateType = request.RateType == "FX" ? InterestRateType.Fixed : InterestRateType.Floating;
        var interest = LoanInterest.Create(
            0, request.LoanId, rateType, request.Percentage,
            request.FloatTypeId, request.EffectiveDate);

        loan.AddInterest(interest);
        await _interestRepository.AddAsync(interest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InterestDto(
            interest.IntId, interest.IntLoanId, interest.IntRateType,
            interest.IntPer, interest.IntFloatTypeId, interest.IntEffDate, interest.IntClsDate);
    }
}
