using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PfiHistories.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PfiHistories.Queries.GetByHlthNum;

public record GetPfiHistoriesByHlthNumQuery(decimal HlthNum) : IRequest<IReadOnlyList<PfiHistoryDto>>;

public class GetPfiHistoriesByHlthNumQueryHandler : IRequestHandler<GetPfiHistoriesByHlthNumQuery, IReadOnlyList<PfiHistoryDto>>
{
    private readonly IUnitOfWork _uow;
    public GetPfiHistoriesByHlthNumQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PfiHistoryDto>> Handle(GetPfiHistoriesByHlthNumQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.PfiHistories.GetByHlthNumAsync(request.HlthNum, cancellationToken);
        return items.Select(CreatePfiHistoryCommandHandler.MapToDto).ToList();
    }
}
