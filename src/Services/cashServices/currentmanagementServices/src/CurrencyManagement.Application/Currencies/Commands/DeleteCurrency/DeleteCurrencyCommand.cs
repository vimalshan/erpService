using MediatR;
using CurrencyManagement.Domain.Interfaces;

namespace CurrencyManagement.Application.Currencies.Commands.DeleteCurrency;

/// <summary>
/// Command to delete a currency
/// </summary>
public record DeleteCurrencyCommand(long CurrencyId) : IRequest<Unit>;

/// <summary>
/// Handler for DeleteCurrencyCommand
/// </summary>
public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, Unit>
{
    private readonly ICurrencyRepository _repository;

    public DeleteCurrencyCommandHandler(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.CurrencyId, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Currency with ID {request.CurrencyId} not found");

        await _repository.DeleteAsync(request.CurrencyId, cancellationToken);

        return Unit.Value;
    }
}
