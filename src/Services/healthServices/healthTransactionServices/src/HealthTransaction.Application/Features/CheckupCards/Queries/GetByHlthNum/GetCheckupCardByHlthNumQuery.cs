using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.CheckupCards.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.CheckupCards.Queries.GetByHlthNum;

public record GetCheckupCardByHlthNumQuery(decimal HlthNum) : IRequest<CheckupCardDto?>;

public class GetCheckupCardByHlthNumQueryHandler : IRequestHandler<GetCheckupCardByHlthNumQuery, CheckupCardDto?>
{
    private readonly IUnitOfWork _uow;
    public GetCheckupCardByHlthNumQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CheckupCardDto?> Handle(GetCheckupCardByHlthNumQuery request, CancellationToken cancellationToken)
    {
        var card = await _uow.CheckupCards.GetByHlthNumAsync(request.HlthNum, cancellationToken);
        return card is null ? null : CreateCheckupCardCommandHandler.MapToDto(card);
    }
}
