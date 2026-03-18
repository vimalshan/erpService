using MediatR;

namespace BankService.Application.Commands.Cheques;

public record ClearChequeCommand(long ChequeId, DateTime ClearedDate) : IRequest<bool>;
