using AccountingService.Application.Common.Exceptions;
using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using AccountingService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Application.Features.MainAccounts.Commands.CreateMainAccount;

public class CreateMainAccountCommandHandler : IRequestHandler<CreateMainAccountCommand, MainAccountDto>
{
    private readonly IApplicationDbContext _context;

    public CreateMainAccountCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<MainAccountDto> Handle(CreateMainAccountCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.MainAccounts
            .AnyAsync(m => m.MainAccountCode == request.MainAccountCode, cancellationToken);

        if (exists)
            throw new InvalidOperationException($"Account code '{request.MainAccountCode}' already exists.");

        var entity = MainAccount.Create(request.MainAccountCode, request.MainAccountName, request.MainAccountShrtName);
        _context.MainAccounts.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new MainAccountDto(entity.MainAccountCode, entity.MainAccountName,
            entity.MainAccountShrtName, entity.MainClosureFlag);
    }
}
