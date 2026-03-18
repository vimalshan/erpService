using FillingOperationService.Application.DTOs;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingCapacities.Queries.GetFillingCapacity;

public class GetFillingCapacityQueryHandler(IFillingCapacityRepository repository)
    : IRequestHandler<GetFillingCapacityQuery, IEnumerable<FillingCapacityDto>>
{
    public async Task<IEnumerable<FillingCapacityDto>> Handle(GetFillingCapacityQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductId.HasValue)
        {
            var single = await repository.GetByGroupAndProductAsync(request.FillingPointGroupId, request.ProductId.Value, cancellationToken);
            return single is null ? [] : [new FillingCapacityDto(single.FillingPointGroupId, single.MainProductId, single.PackageTypeId, single.ItemCapacityId, single.CapacityPerShift, single.UsagePriority)];
        }
        var list = await repository.GetByGroupIdAsync(request.FillingPointGroupId, cancellationToken);
        return list.Select(c => new FillingCapacityDto(c.FillingPointGroupId, c.MainProductId, c.PackageTypeId, c.ItemCapacityId, c.CapacityPerShift, c.UsagePriority));
    }
}
