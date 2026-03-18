using AutoMapper;
using MediatR;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.Features.ControlPoints.Queries;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Interfaces;
using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.ControlPoints.Handlers;

public class GetControlPointByIdQueryHandler : IRequestHandler<GetControlPointByIdQuery, CommandResult<ControlPointDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetControlPointByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<ControlPointDto>> Handle(GetControlPointByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<ControlPoint>();
            var controlPoint = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (controlPoint == null)
                return CommandResult<ControlPointDto>.Failure("Control Point not found");

            var dto = _mapper.Map<ControlPointDto>(controlPoint);
            return CommandResult<ControlPointDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return CommandResult<ControlPointDto>.Failure($"Error retrieving control point: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class GetControlPointsByEmployeeQueryHandler : IRequestHandler<GetControlPointsByEmployeeQuery, CommandResult<List<ControlPointDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetControlPointsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<List<ControlPointDto>>> Handle(GetControlPointsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<ControlPoint>();
            var controlPoints = repository.AsQueryable()
                .Where(x => x.EmployeeSysId == request.EmployeeSysId && x.DDYearId == request.DDYearId)
                .ToList();

            var dtos = _mapper.Map<List<ControlPointDto>>(controlPoints);
            return CommandResult<List<ControlPointDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return CommandResult<List<ControlPointDto>>.Failure($"Error retrieving control points: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class GetAllControlPointsQueryHandler : IRequestHandler<GetAllControlPointsQuery, CommandResult<List<ControlPointDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllControlPointsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<List<ControlPointDto>>> Handle(GetAllControlPointsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<ControlPoint>();
            var controlPoints = repository.AsQueryable()
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<List<ControlPointDto>>(controlPoints);
            return CommandResult<List<ControlPointDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return CommandResult<List<ControlPointDto>>.Failure($"Error retrieving control points: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}
