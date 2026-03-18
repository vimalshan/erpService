using MediatR;
using LoanApplication.Application.Commands;
using LoanApplication.Domain.Aggregates;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Domain.ValueObjects;

namespace LoanApplication.Application.CommandHandlers;

/// <summary>
/// Handler for CreateLoanApplicationCommand
/// </summary>
public class CreateLoanApplicationCommandHandler : IRequestHandler<CreateLoanApplicationCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoanEligibilityService _eligibilityService;
    private readonly IPublisher _publisher;

    public CreateLoanApplicationCommandHandler(
        IUnitOfWork unitOfWork,
        ILoanEligibilityService eligibilityService,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _eligibilityService = eligibilityService;
        _publisher = publisher;
    }

    public async Task<long> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        // Check eligibility
        var isEligible = await _eligibilityService.IsEligibleAsync(request.EmployeeId, request.LoanId, cancellationToken);
        if (!isEligible)
            throw new InvalidOperationException("Employee is not eligible for this loan");

        // Create aggregate
        var loanSource = LoanSource.FromValue(request.Source);
        var amount = Money.Create(request.Amount);

        var loanApplication = LoanApplicationAggregate.Create(
            request.EmployeeId,
            request.LoanId,
            request.AppliedBy,
            loanSource,
            amount,
            request.Reason,
            request.GuarantorId,
            request.TenureMonths);

        if (request.SecondGuarantorId.HasValue && request.SecondGuarantorId.Value > 0)
        {
            loanApplication.SetSecondGuarantor(request.SecondGuarantorId.Value, request.AppliedBy);
        }

        // Save to repository
        await _unitOfWork.LoanApplications.AddAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain events
        foreach (var domainEvent in loanApplication.GetDomainEvents())
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return loanApplication.Id;
    }
}

/// <summary>
/// Handler for SubmitLoanApplicationCommand
/// </summary>
public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public SubmitLoanApplicationCommandHandler(IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<bool> Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.Submit(request.SubmittedBy);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in loanApplication.GetDomainEvents())
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return true;
    }
}

/// <summary>
/// Handler for ApproveLoanApplicationCommand
/// </summary>
public class ApproveLoanApplicationCommandHandler : IRequestHandler<ApproveLoanApplicationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ApproveLoanApplicationCommandHandler(IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<bool> Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.Approve(request.ApprovedBy, request.Remarks);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in loanApplication.GetDomainEvents())
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return true;
    }
}

/// <summary>
/// Handler for RejectLoanApplicationCommand
/// </summary>
public class RejectLoanApplicationCommandHandler : IRequestHandler<RejectLoanApplicationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public RejectLoanApplicationCommandHandler(IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<bool> Handle(RejectLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.Reject(request.RejectedBy, request.Remarks);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in loanApplication.GetDomainEvents())
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return true;
    }
}

/// <summary>
/// Handler for DisburseLoanCommand
/// </summary>
public class DisburseLoanCommandHandler : IRequestHandler<DisburseLoanCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public DisburseLoanCommandHandler(IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<bool> Handle(DisburseLoanCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.Disburse(request.DisbursingBy);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in loanApplication.GetDomainEvents())
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return true;
    }
}

/// <summary>
/// Handler for SetSecondGuarantorCommand
/// </summary>
public class SetSecondGuarantorCommandHandler : IRequestHandler<SetSecondGuarantorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetSecondGuarantorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetSecondGuarantorCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.SetSecondGuarantor(request.SecondGuarantorId, request.ModifiedBy);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

/// <summary>
/// Handler for MarkForSpecialSanctionCommand
/// </summary>
public class MarkForSpecialSanctionCommandHandler : IRequestHandler<MarkForSpecialSanctionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkForSpecialSanctionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MarkForSpecialSanctionCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _unitOfWork.LoanApplications.GetByIdAsync(request.LoanApplicationId, cancellationToken);
        if (loanApplication == null)
            throw new KeyNotFoundException($"Loan application with ID {request.LoanApplicationId} not found");

        loanApplication.MarkForSpecialSanction(request.Sanctioned, request.ModifiedBy);

        await _unitOfWork.LoanApplications.UpdateAsync(loanApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
