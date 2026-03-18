using MediatR;
using CashManagement.Application.DTOs;
using CashManagement.Application.Queries.CashUnit;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.Queries.CashUnit;

public class GetAllCashUnitsHandler : IRequestHandler<GetAllCashUnitsQuery, IEnumerable<CashUnitDto>>
{
    private readonly ICashUnitRepository _repository;
    public GetAllCashUnitsHandler(ICashUnitRepository repository) => _repository = repository;

    public async Task<IEnumerable<CashUnitDto>> Handle(GetAllCashUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await _repository.GetAllAsync(cancellationToken);
        return units.Select(u => new CashUnitDto(u.Id, u.Name, u.Code, u.Location,
            u.InChargeEmployeeId, u.OpeningBalance, u.Status.ToString(), 0, u.CreatedOn));
    }
}

public class GetCashUnitByIdHandler : IRequestHandler<GetCashUnitByIdQuery, CashUnitDto?>
{
    private readonly ICashUnitRepository _repository;
    public GetCashUnitByIdHandler(ICashUnitRepository repository) => _repository = repository;

    public async Task<CashUnitDto?> Handle(GetCashUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var u = await _repository.GetByIdAsync(request.CashUnitId, cancellationToken);
        if (u is null) return null;
        return new CashUnitDto(u.Id, u.Name, u.Code, u.Location,
            u.InChargeEmployeeId, u.OpeningBalance, u.Status.ToString(), 0, u.CreatedOn);
    }
}

public class GetCashInHandHandler : IRequestHandler<GetCashInHandQuery, CashBalanceDto>
{
    private readonly ICashUnitRepository _repository;
    public GetCashInHandHandler(ICashUnitRepository repository) => _repository = repository;

    public async Task<CashBalanceDto> Handle(GetCashInHandQuery request, CancellationToken cancellationToken)
    {
        var unit = await _repository.GetByIdAsync(request.CashUnitId, cancellationToken);
        var balance = await _repository.GetCashInHandAsync(request.CashUnitId, request.AsOfDate, cancellationToken);
        return new CashBalanceDto(request.CashUnitId, unit?.Name ?? string.Empty, balance, request.AsOfDate);
    }
}

public class GetCashTransactionsByUnitHandler : IRequestHandler<GetCashTransactionsByUnitQuery, IEnumerable<CashTransactionDto>>
{
    private readonly ICashTransactionRepository _repository;
    public GetCashTransactionsByUnitHandler(ICashTransactionRepository repository) => _repository = repository;

    public async Task<IEnumerable<CashTransactionDto>> Handle(GetCashTransactionsByUnitQuery request, CancellationToken cancellationToken)
    {
        var txns = await _repository.GetByUnitAsync(request.CashUnitId, request.From, request.To, cancellationToken);
        return txns.Select(t => new CashTransactionDto(t.CashTxnId, t.CashUnitId, t.TxnType.ToString(),
            t.Amount, t.Source, t.PayeeId, t.RefNo, t.TxnDate, t.Remarks,
            t.Status.ToString(), t.AuthorizedBy, t.CreatedBy, t.CreatedOn));
    }
}
