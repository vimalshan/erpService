using FillingOperationService.Application.DTOs;
using MediatR;

namespace FillingOperationService.Application.FillingLines.Queries.GetFillingLines;

public record GetFillingLinesQuery(int PlantId) : IRequest<IEnumerable<FillingLineDto>>;
