using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.MainAccounts.Commands.CreateMainAccount;

public record CreateMainAccountCommand(
    string MainAccountCode,
    string MainAccountName,
    string? MainAccountShrtName
) : IRequest<MainAccountDto>;
