using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByDateRange;

public record GetPreEmploymentCheckupsByDateRangeQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<PreEmploymentCheckupDto>>;

public class GetPreEmploymentCheckupsByDateRangeQueryHandler
    : IRequestHandler<GetPreEmploymentCheckupsByDateRangeQuery, IReadOnlyList<PreEmploymentCheckupDto>>
{
    private readonly IUnitOfWork _uow;
    public GetPreEmploymentCheckupsByDateRangeQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> Handle(GetPreEmploymentCheckupsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.PreEmploymentCheckups.GetByDateRangeAsync(request.From, request.To, cancellationToken);
        return items.Select(CreatePreEmploymentCheckupCommandHandler.MapToDto).ToList();
    }
}
