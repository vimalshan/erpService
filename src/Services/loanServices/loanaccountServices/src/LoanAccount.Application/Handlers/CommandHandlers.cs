using Ardalis.GuardClauses;
using LoanAccount.Application.Commands;
using LoanAccount.Application.Services;
using MediatR;

namespace LoanAccount.Application.Handlers;

/// <summary>
/// Handler for CreateLoanCommand
/// </summary>
public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, long>
{
    private readonly LoanApplicationService _loanService;

    public CreateLoanCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<long> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        => _loanService.CreateLoanAsync(request, cancellationToken);
}

/// <summary>
/// Handler for ApproveLoanCommand
/// </summary>
public class ApproveLoanCommandHandler : IRequestHandler<ApproveLoanCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public ApproveLoanCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(ApproveLoanCommand request, CancellationToken cancellationToken)
        => _loanService.ApproveLoanAsync(request, cancellationToken);
}

/// <summary>
/// Handler for DisburseLoanCommand
/// </summary>
public class DisburseLoanCommandHandler : IRequestHandler<DisburseLoanCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public DisburseLoanCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(DisburseLoanCommand request, CancellationToken cancellationToken)
        => _loanService.DisburseLoanAsync(request, cancellationToken);
}

/// <summary>
/// Handler for CreateLoanInstallmentsCommand
/// </summary>
public class CreateLoanInstallmentsCommandHandler : IRequestHandler<CreateLoanInstallmentsCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public CreateLoanInstallmentsCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(CreateLoanInstallmentsCommand request, CancellationToken cancellationToken)
        => _loanService.CreateInstallmentsAsync(request, cancellationToken);
}

/// <summary>
/// Handler for RecordEMIPaymentCommand
/// </summary>
public class RecordEMIPaymentCommandHandler : IRequestHandler<RecordEMIPaymentCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public RecordEMIPaymentCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(RecordEMIPaymentCommand request, CancellationToken cancellationToken)
        => _loanService.RecordEMIPaymentAsync(request, cancellationToken);
}

/// <summary>
/// Handler for SettleLoanCommand
/// </summary>
public class SettleLoanCommandHandler : IRequestHandler<SettleLoanCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public SettleLoanCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(SettleLoanCommand request, CancellationToken cancellationToken)
        => _loanService.SettleLoanAsync(request, cancellationToken);
}

/// <summary>
/// Handler for CloseLoanCommand
/// </summary>
public class CloseLoanCommandHandler : IRequestHandler<CloseLoanCommand, bool>
{
    private readonly LoanApplicationService _loanService;

    public CloseLoanCommandHandler(LoanApplicationService loanService)
    {
        _loanService = Guard.Against.Null(loanService, nameof(loanService));
    }

    public Task<bool> Handle(CloseLoanCommand request, CancellationToken cancellationToken)
        => _loanService.CloseLoanAsync(request, cancellationToken);
}
