using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Queries.Cheques;

public record GetChequeByIdQuery(long ChequeId) : IRequest<ChequeDetailDto?>;

public record GetChequesByStatusQuery(string Status) : IRequest<IReadOnlyList<ChequeDetailDto>>;

public record GetAllChequesQuery : IRequest<IReadOnlyList<ChequeDetailDto>>;
