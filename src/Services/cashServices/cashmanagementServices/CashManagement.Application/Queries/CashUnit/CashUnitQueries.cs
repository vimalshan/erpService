using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Queries.CashUnit;

public record GetAllCashUnitsQuery : IRequest<IEnumerable<CashUnitDto>>;
public record GetCashUnitByIdQuery(long CashUnitId) : IRequest<CashUnitDto?>;
public record GetCashInHandQuery(long CashUnitId, DateTime AsOfDate) : IRequest<CashBalanceDto>;
public record GetCashTransactionsByUnitQuery(long CashUnitId, DateTime From, DateTime To) : IRequest<IEnumerable<CashTransactionDto>>;
