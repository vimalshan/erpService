using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Queries.ChequeRegister;

public record GetChequesByAccountQuery(long BankAccountId) : IRequest<IEnumerable<ChequeDto>>;
public record GetChequeByIdQuery(long ChequeId) : IRequest<ChequeDto?>;
