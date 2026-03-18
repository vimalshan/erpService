using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Queries.BankReconciliation;

public record GetReconciliationHistoryQuery(long BankAccountId) : IRequest<IEnumerable<BankReconciliationDto>>;
public record GetReconciliationByIdQuery(long ReconId) : IRequest<BankReconciliationDto?>;
