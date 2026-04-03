using Ardalis.GuardClauses;
using AutoMapper;
using LoanAccount.Application.DTOs;
using LoanAccount.Application.Queries;
using LoanAccount.Domain.Interfaces;
using MediatR;

namespace LoanAccount.Application.Handlers;

/// <summary>
/// Handler for GetLoanByNumberQuery
/// </summary>
public class GetLoanByNumberQueryHandler : IRequestHandler<GetLoanByNumberQuery, LoanResponse?>
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly IMapper _mapper;

    public GetLoanByNumberQueryHandler(ILoanMainRepository loanRepository, IMapper mapper)
    {
        _loanRepository = Guard.Against.Null(loanRepository, nameof(loanRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<LoanResponse?> Handle(GetLoanByNumberQuery request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByLoanNumberAsync(request.LoanNo, cancellationToken);
        return loan is null ? null : _mapper.Map<LoanResponse>(loan);
    }
}

/// <summary>
/// Handler for GetEmployeeLoansQuery
/// </summary>
public class GetEmployeeLoansQueryHandler : IRequestHandler<GetEmployeeLoansQuery, IEnumerable<LoanResponse>>
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly IMapper _mapper;

    public GetEmployeeLoansQueryHandler(ILoanMainRepository loanRepository, IMapper mapper)
    {
        _loanRepository = Guard.Against.Null(loanRepository, nameof(loanRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IEnumerable<LoanResponse>> Handle(GetEmployeeLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetByEmployeeAsync(request.EmployeeId, cancellationToken);
        return _mapper.Map<IEnumerable<LoanResponse>>(loans);
    }
}

/// <summary>
/// Handler for GetUnitLoansQuery
/// </summary>
public class GetUnitLoansQueryHandler : IRequestHandler<GetUnitLoansQuery, IEnumerable<LoanResponse>>
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly IMapper _mapper;

    public GetUnitLoansQueryHandler(ILoanMainRepository loanRepository, IMapper mapper)
    {
        _loanRepository = Guard.Against.Null(loanRepository, nameof(loanRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IEnumerable<LoanResponse>> Handle(GetUnitLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetByUnitAsync(request.UnitId, cancellationToken);
        return _mapper.Map<IEnumerable<LoanResponse>>(loans);
    }
}

/// <summary>
/// Handler for GetActiveLoansQuery
/// </summary>
public class GetActiveLoansQueryHandler : IRequestHandler<GetActiveLoansQuery, IEnumerable<LoanResponse>>
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly IMapper _mapper;

    public GetActiveLoansQueryHandler(ILoanMainRepository loanRepository, IMapper mapper)
    {
        _loanRepository = Guard.Against.Null(loanRepository, nameof(loanRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IEnumerable<LoanResponse>> Handle(GetActiveLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetActiveLoansAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LoanResponse>>(loans);
    }
}

/// <summary>
/// Handler for GetLoanDetailsQuery
/// </summary>
public class GetLoanDetailsQueryHandler : IRequestHandler<GetLoanDetailsQuery, LoanDetailsResponse?>
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly ILoanInstallmentRepository _installmentRepository;
    private readonly ILoanLedgerRepository _ledgerRepository;
    private readonly IMapper _mapper;

    public GetLoanDetailsQueryHandler(
        ILoanMainRepository loanRepository,
        ILoanInstallmentRepository installmentRepository,
        ILoanLedgerRepository ledgerRepository,
        IMapper mapper)
    {
        _loanRepository = Guard.Against.Null(loanRepository, nameof(loanRepository));
        _installmentRepository = Guard.Against.Null(installmentRepository, nameof(installmentRepository));
        _ledgerRepository = Guard.Against.Null(ledgerRepository, nameof(ledgerRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<LoanDetailsResponse?> Handle(GetLoanDetailsQuery request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByLoanNumberAsync(request.LoanNo, cancellationToken);
        if (loan is null) return null;

        var installments = await _installmentRepository.GetByLoanNoAsync(loan.LoanNo, cancellationToken);
        var ledgerEntries = await _ledgerRepository.GetByLoanNoAsync(loan.LoanNo, cancellationToken);

        return new LoanDetailsResponse
        {
            LoanNo = loan.LoanNo,
            EmployeeId = loan.EmpSysId,
            PrincipalAmount = loan.PrincipalAmount.Amount,
            DisbursedAmount = loan.DisbursedAmount.Amount,
            OutstandingAmount = loan.PrincipalOutstanding.Amount,
            Status = loan.LoanStatus.Status,
            LoanDate = loan.LoanDate,
            ClosureDate = loan.LoanClosureDate,
            Installments = _mapper.Map<IEnumerable<InstallmentResponse>>(installments),
            LedgerEntries = _mapper.Map<IEnumerable<LoanLedgerEntryResponse>>(ledgerEntries)
        };
    }
}

/// <summary>
/// Handler for GetLoanInstallmentsQuery
/// </summary>
public class GetLoanInstallmentsQueryHandler : IRequestHandler<GetLoanInstallmentsQuery, IEnumerable<InstallmentResponse>>
{
    private readonly ILoanInstallmentRepository _installmentRepository;
    private readonly IMapper _mapper;

    public GetLoanInstallmentsQueryHandler(ILoanInstallmentRepository installmentRepository, IMapper mapper)
    {
        _installmentRepository = Guard.Against.Null(installmentRepository, nameof(installmentRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IEnumerable<InstallmentResponse>> Handle(GetLoanInstallmentsQuery request, CancellationToken cancellationToken)
    {
        var installments = await _installmentRepository.GetByLoanNoAsync(request.LoanNo, cancellationToken);
        return _mapper.Map<IEnumerable<InstallmentResponse>>(installments);
    }
}

/// <summary>
/// Handler for GetLoanLedgerEntriesQuery
/// </summary>
public class GetLoanLedgerEntriesQueryHandler : IRequestHandler<GetLoanLedgerEntriesQuery, IEnumerable<LoanLedgerEntryResponse>>
{
    private readonly ILoanLedgerRepository _ledgerRepository;
    private readonly IMapper _mapper;

    public GetLoanLedgerEntriesQueryHandler(ILoanLedgerRepository ledgerRepository, IMapper mapper)
    {
        _ledgerRepository = Guard.Against.Null(ledgerRepository, nameof(ledgerRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IEnumerable<LoanLedgerEntryResponse>> Handle(GetLoanLedgerEntriesQuery request, CancellationToken cancellationToken)
    {
        var entries = await _ledgerRepository.GetByLoanNoAsync(request.LoanNo, cancellationToken);
        return _mapper.Map<IEnumerable<LoanLedgerEntryResponse>>(entries);
    }
}
