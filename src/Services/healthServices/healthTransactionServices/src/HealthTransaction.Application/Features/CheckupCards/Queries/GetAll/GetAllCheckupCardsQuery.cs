using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.CheckupCards.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.CheckupCards.Queries.GetAll;

public record GetAllCheckupCardsQuery : IRequest<IReadOnlyList<CheckupCardDto>>;

public class GetAllCheckupCardsQueryHandler : IRequestHandler<GetAllCheckupCardsQuery, IReadOnlyList<CheckupCardDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllCheckupCardsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<CheckupCardDto>> Handle(GetAllCheckupCardsQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.CheckupCards.GetAllAsync(cancellationToken);
        return items.Select(CreateCheckupCardCommandHandler.MapToDto).ToList();
    }
}
