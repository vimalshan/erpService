using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateOrderMap;

public sealed record CreateOrderMapCommand(
    decimal? TiedOrderDetailId, decimal? ActualLineId,
    int? MappingQuantity, int? ModifiedByUserId) : IRequest<int>;

public sealed class CreateOrderMapCommandValidator : AbstractValidator<CreateOrderMapCommand>
{
    public CreateOrderMapCommandValidator()
    {
        RuleFor(x => x.MappingQuantity).GreaterThanOrEqualTo(0).When(x => x.MappingQuantity.HasValue);
    }
}

public sealed class CreateOrderMapCommandHandler(IOrderMapRepository repository)
    : IRequestHandler<CreateOrderMapCommand, int>
{
    public async Task<int> Handle(CreateOrderMapCommand request, CancellationToken cancellationToken)
    {
        var entity = ActualOrderMapEntity.Create(
            request.TiedOrderDetailId, request.ActualLineId,
            request.MappingQuantity, request.ModifiedByUserId);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
