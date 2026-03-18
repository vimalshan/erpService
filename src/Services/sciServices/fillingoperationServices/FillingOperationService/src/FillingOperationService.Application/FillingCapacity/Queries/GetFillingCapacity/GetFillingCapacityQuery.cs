using FillingOperationService.Application.DTOs;
using MediatR;

namespace FillingOperationService.Application.FillingCapacities.Queries.GetFillingCapacity;

public record GetFillingCapacityQuery(int FillingPointGroupId, int? ProductId = null) : IRequest<IEnumerable<FillingCapacityDto>>;
