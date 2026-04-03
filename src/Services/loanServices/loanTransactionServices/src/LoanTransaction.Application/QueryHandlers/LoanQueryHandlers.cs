using MediatR;
using AutoMapper;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using LoanTransaction.Domain.Interfaces;

namespace LoanTransaction.Application.QueryHandlers;

public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoanByIdQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<LoanDto?> Handle(GetLoanByIdQuery q, CancellationToken ct)
    {
        var loan = await _uow.Loans.GetByIdAsync(q.LoanNo, ct);
        return loan is null ? null : _mapper.Map<LoanDto>(loan);
    }
}

public class GetLoansByEmployeeQueryHandler : IRequestHandler<GetLoansByEmployeeQuery, IEnumerable<LoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoansByEmployeeQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanDto>> Handle(GetLoansByEmployeeQuery q, CancellationToken ct)
    {
        var loans = q.ActiveOnly
            ? await _uow.Loans.GetActiveByEmployeeIdAsync(q.EmployeeId, ct)
            : await _uow.Loans.GetByEmployeeIdAsync(q.EmployeeId, ct);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}

public class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, PagedLoanResultDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllLoansQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<PagedLoanResultDto> Handle(GetAllLoansQuery q, CancellationToken ct)
    {
        var loans = await _uow.Loans.GetAllAsync(q.PageNumber, q.PageSize, ct);
        var total = await _uow.Loans.GetTotalCountAsync(ct);
        return new PagedLoanResultDto
        {
            Items = _mapper.Map<IEnumerable<LoanDto>>(loans),
            TotalCount = total,
            PageNumber = q.PageNumber,
            PageSize = q.PageSize
        };
    }
}

public class GetInstallmentScheduleQueryHandler : IRequestHandler<GetInstallmentScheduleQuery, IEnumerable<LoanInstallmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetInstallmentScheduleQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanInstallmentDto>> Handle(GetInstallmentScheduleQuery q, CancellationToken ct)
    {
        var items = await _uow.Installments.GetByLoanNoAsync(q.LoanNo, ct);
        return _mapper.Map<IEnumerable<LoanInstallmentDto>>(items);
    }
}

public class GetPendingInstallmentsQueryHandler : IRequestHandler<GetPendingInstallmentsQuery, IEnumerable<LoanInstallmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPendingInstallmentsQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanInstallmentDto>> Handle(GetPendingInstallmentsQuery q, CancellationToken ct)
    {
        var items = await _uow.Installments.GetPendingByLoanNoAsync(q.LoanNo, ct);
        return _mapper.Map<IEnumerable<LoanInstallmentDto>>(items);
    }
}

public class GetLoanLedgerQueryHandler : IRequestHandler<GetLoanLedgerQuery, IEnumerable<LoanLedgerDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoanLedgerQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanLedgerDto>> Handle(GetLoanLedgerQuery q, CancellationToken ct)
    {
        var items = await _uow.LedgerEntries.GetByLoanNoAsync(q.LoanNo, ct);
        return _mapper.Map<IEnumerable<LoanLedgerDto>>(items);
    }
}

public class GetLoanLedgerByEmployeeQueryHandler : IRequestHandler<GetLoanLedgerByEmployeeQuery, IEnumerable<LoanLedgerDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoanLedgerByEmployeeQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanLedgerDto>> Handle(GetLoanLedgerByEmployeeQuery q, CancellationToken ct)
    {
        var items = await _uow.LedgerEntries.GetByEmployeeIdAsync(q.EmployeeId, ct);
        return _mapper.Map<IEnumerable<LoanLedgerDto>>(items);
    }
}

public class GetLoanSettlementsQueryHandler : IRequestHandler<GetLoanSettlementsQuery, IEnumerable<LoanSettlementDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetLoanSettlementsQueryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<IEnumerable<LoanSettlementDto>> Handle(GetLoanSettlementsQuery q, CancellationToken ct)
    {
        var items = await _uow.Settlements.GetByLoanNoAsync(q.LoanNo, ct);
        return _mapper.Map<IEnumerable<LoanSettlementDto>>(items);
    }
}

public class CalculateEmiQueryHandler : IRequestHandler<CalculateEmiQuery, EmiCalculationResultDto>
{
    private readonly IEmiCalculatorService _emiCalc;
    private readonly IMapper _mapper;

    public CalculateEmiQueryHandler(IEmiCalculatorService emiCalc, IMapper mapper)
    {
        _emiCalc = emiCalc;
        _mapper = mapper;
    }

    public Task<EmiCalculationResultDto> Handle(CalculateEmiQuery q, CancellationToken ct)
    {
        var emi = _emiCalc.CalculateEmi(q.PrincipalAmount, q.RatePerAnnum, q.TenureMonths);
        var schedule = _emiCalc.GenerateSchedule(q.PrincipalAmount, q.RatePerAnnum, q.TenureMonths, DateTime.Today.AddMonths(1)).ToList();
        var totalPayable = emi * q.TenureMonths;

        return Task.FromResult(new EmiCalculationResultDto
        {
            EmiAmount = emi,
            PrincipalAmount = q.PrincipalAmount,
            RatePerAnnum = q.RatePerAnnum,
            TenureMonths = q.TenureMonths,
            TotalInterest = totalPayable - q.PrincipalAmount,
            TotalPayable = totalPayable,
            Schedule = _mapper.Map<IEnumerable<EmiScheduleItemDto>>(schedule)
        });
    }
}
