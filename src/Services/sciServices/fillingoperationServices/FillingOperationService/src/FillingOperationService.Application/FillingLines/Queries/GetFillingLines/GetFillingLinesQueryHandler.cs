using FillingOperationService.Application.DTOs;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingLines.Queries.GetFillingLines;

public class GetFillingLinesQueryHandler(IFillingLineRepository repository)
    : IRequestHandler<GetFillingLinesQuery, IEnumerable<FillingLineDto>>
{
    public async Task<IEnumerable<FillingLineDto>> Handle(GetFillingLinesQuery request, CancellationToken cancellationToken)
    {
        var lines = await repository.GetByPlantIdAsync(request.PlantId, cancellationToken);
        return lines.Select(l => new FillingLineDto(
            l.FillingLineId, l.FillingPlantId, l.FillingLineName,
            l.NoOfFillingPoints, l.PackageTypeId, l.IsClosed, l.CreationDate));
    }
}
