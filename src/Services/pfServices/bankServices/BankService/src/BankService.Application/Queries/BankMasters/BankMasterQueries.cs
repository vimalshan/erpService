using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Queries.BankMasters;

public record GetAllBankMastersQuery : IRequest<IReadOnlyList<BankMasterDto>>;

public record GetBankMasterByCodeQuery(string TrustCode, string BankCode) : IRequest<BankMasterDto?>;

public record GetBankMastersByTrustCodeQuery(string TrustCode) : IRequest<IReadOnlyList<BankMasterDto>>;
