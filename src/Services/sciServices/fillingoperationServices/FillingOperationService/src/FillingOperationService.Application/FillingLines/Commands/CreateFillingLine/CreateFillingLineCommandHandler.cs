using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingLines.Commands.CreateFillingLine;

public class CreateFillingLineCommandHandler(IFillingLineRepository repository)
    : IRequestHandler<CreateFillingLineCommand, int>
{
    public async Task<int> Handle(CreateFillingLineCommand request, CancellationToken cancellationToken)
    {
        var line = FillingLine.Create(request.FillingPlantId, request.FillingLineName, request.NoOfFillingPoints, request.PackageTypeId, request.CreatedBy);
        await repository.AddAsync(line, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return line.FillingLineId;
    }
}
