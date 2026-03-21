using ConfigService.Application.DTOs;
using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Currencies.Commands;

public class CreateCurrencyHandler(ICurrencyRepository repo, IUnitOfWork uow) : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
{
    public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken ct)
    {
        var currency = Currency.Create(0, request.CurrencyCode, request.CurrencyName, request.CurrencySymbol);
        await repo.AddAsync(currency, ct);
        await uow.SaveChangesAsync(ct);
        return new CurrencyDto(currency.Id, currency.CurrencyCode, currency.CurrencyName, currency.CurrencySymbol);
    }
}

public class UpdateCurrencyHandler(ICurrencyRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
{
    public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken ct)
    {
        var currency = await repo.GetByIdAsync(request.CurrencyId, ct)
            ?? throw new KeyNotFoundException($"Currency {request.CurrencyId} not found.");
        currency.Update(request.CurrencyCode, request.CurrencyName, request.CurrencySymbol);
        await repo.UpdateAsync(currency, ct);
        await uow.SaveChangesAsync(ct);
        return new CurrencyDto(currency.Id, currency.CurrencyCode, currency.CurrencyName, currency.CurrencySymbol);
    }
}

public class DeleteCurrencyHandler(ICurrencyRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteCurrencyCommand, bool>
{
    public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken ct)
    {
        var currency = await repo.GetByIdAsync(request.CurrencyId, ct);
        if (currency is null) return false;
        await repo.DeleteAsync(currency, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
