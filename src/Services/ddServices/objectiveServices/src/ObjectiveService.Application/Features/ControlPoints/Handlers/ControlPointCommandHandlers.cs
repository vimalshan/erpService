using AutoMapper;
using MediatR;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.Features.ControlPoints.Commands;
using ObjectiveService.Application.Interfaces;
using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.ControlPoints.Handlers;

public class CreateControlPointCommandHandler : IRequestHandler<CreateControlPointCommand, CommandResult<decimal>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateControlPointCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<decimal>> Handle(CreateControlPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var controlPoint = new ControlPoint(
                request.EmployeeSysId,
                request.DDYearId,
                request.Source,
                request.RefId,
                request.SerialNumber,
                request.Description,
                request.Category,
                request.UnitOfMeasurement,
                request.UnitFrom,
                request.UnitTo,
                request.VersionNumber,
                request.Weightage,
                request.AccountabilityId
            );

            var repository = _unitOfWork.Repository<ControlPoint>();
            await repository.AddAsync(controlPoint, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult<decimal>.Success(controlPoint.Id, "Control Point created successfully");
        }
        catch (Exception ex)
        {
            return CommandResult<decimal>.Failure($"Error creating control point: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class UpdateControlPointCommandHandler : IRequestHandler<UpdateControlPointCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateControlPointCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(UpdateControlPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<ControlPoint>();
            var controlPoint = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (controlPoint == null)
                return CommandResult.Failure("Control Point not found");

            controlPoint.Update(
                request.Description,
                request.UnitFrom,
                request.UnitTo,
                request.Weightage
            );

            await repository.UpdateAsync(controlPoint, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Control Point updated successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error updating control point: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class DeleteControlPointCommandHandler : IRequestHandler<DeleteControlPointCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteControlPointCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(DeleteControlPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<ControlPoint>();
            var controlPoint = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (controlPoint == null)
                return CommandResult.Failure("Control Point not found");

            controlPoint.Delete();

            await repository.UpdateAsync(controlPoint, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Control Point deleted successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error deleting control point: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}
