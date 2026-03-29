using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.DynamicHealthDetails.Commands.Save;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.DynamicHealthDetails.Queries.GetByHlthNum;

public record GetDynamicHealthDetailsByHlthNumQuery(decimal HlthNum) : IRequest<IReadOnlyList<DynamicHealthDetailDto>>;

public class GetDynamicHealthDetailsByHlthNumQueryHandler : IRequestHandler<GetDynamicHealthDetailsByHlthNumQuery, IReadOnlyList<DynamicHealthDetailDto>>
{
    private readonly IUnitOfWork _uow;
    public GetDynamicHealthDetailsByHlthNumQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<DynamicHealthDetailDto>> Handle(GetDynamicHealthDetailsByHlthNumQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.DynamicHealthDetails.GetByHlthNumAsync(request.HlthNum, cancellationToken);
        return items.Select(SaveDynamicHealthDetailsCommandHandler.MapToDto).ToList();
    }
}
