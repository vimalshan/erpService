using MediatR;

namespace FillingOperationService.Application.FillingLines.Commands.CreateFillingLine;

public record CreateFillingLineCommand(
    int FillingPlantId,
    string FillingLineName,
    int NoOfFillingPoints,
    int? PackageTypeId,
    int CreatedBy
) : IRequest<int>;
