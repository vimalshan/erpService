using MediatR;
using LoanManagement.Application.DTOs;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Commands.AddRepaymentSchedule;

public class AddRepaymentScheduleCommandHandler
    : IRequestHandler<AddRepaymentScheduleCommand, List<RepaymentScheduleDto>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IRepaymentRepository _repaymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddRepaymentScheduleCommandHandler(
        ILoanRepository loanRepository,
        IRepaymentRepository repaymentRepository,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _repaymentRepository = repaymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<RepaymentScheduleDto>> Handle(
        AddRepaymentScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new LoanDomainException($"Loan {request.LoanId} not found.");

        var repayments = new List<LoanRepaymentSchedule>();

        for (int i = 0; i < request.Lines.Count; i++)
        {
            var line = request.Lines[i];
            var repayment = LoanRepaymentSchedule.Create(
                0, request.LoanId, line.RepayDate, line.Amount);
            loan.AddRepayment(repayment);
            repayments.Add(repayment);
        }

        await _repaymentRepository.AddRangeAsync(repayments, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return repayments.Select(r => new RepaymentScheduleDto(
            r.RepayId, r.RepayLoanId, r.RepayDate, r.RepayAmt, r.RepayFlag)).ToList();
    }
}
