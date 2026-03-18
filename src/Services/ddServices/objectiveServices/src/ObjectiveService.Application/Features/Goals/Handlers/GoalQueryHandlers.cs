using AutoMapper;
using MediatR;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.Features.Goals.Queries;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Interfaces;
using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.Goals.Handlers;

public class GetGoalByIdQueryHandler : IRequestHandler<GetGoalByIdQuery, CommandResult<GoalDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGoalByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<GoalDto>> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goal = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (goal == null)
                return CommandResult<GoalDto>.Failure("Goal not found");

            var dto = _mapper.Map<GoalDto>(goal);
            return CommandResult<GoalDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return CommandResult<GoalDto>.Failure($"Error retrieving goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class GetGoalsByEmployeeQueryHandler : IRequestHandler<GetGoalsByEmployeeQuery, CommandResult<List<GoalDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGoalsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<List<GoalDto>>> Handle(GetGoalsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goals = repository.AsQueryable()
                .Where(x => x.UserId == request.UserId && x.PinNumber == request.PinNumber)
                .ToList();

            var dtos = _mapper.Map<List<GoalDto>>(goals);
            return CommandResult<List<GoalDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return CommandResult<List<GoalDto>>.Failure($"Error retrieving goals: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class GetGoalsByPeriodQueryHandler : IRequestHandler<GetGoalsByPeriodQuery, CommandResult<List<GoalDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGoalsByPeriodQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<List<GoalDto>>> Handle(GetGoalsByPeriodQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goals = repository.AsQueryable()
                .Where(x => x.PeriodFrom >= request.PeriodFrom && x.PeriodTo <= request.PeriodTo)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<List<GoalDto>>(goals);
            return CommandResult<List<GoalDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return CommandResult<List<GoalDto>>.Failure($"Error retrieving goals: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class GetActiveDraftGoalsQueryHandler : IRequestHandler<GetActiveDraftGoalsQuery, CommandResult<List<GoalDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveDraftGoalsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommandResult<List<GoalDto>>> Handle(GetActiveDraftGoalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goals = repository.AsQueryable()
                .Where(x => x.Status == "N") // N = with appraisee (draft)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<List<GoalDto>>(goals);
            return CommandResult<List<GoalDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return CommandResult<List<GoalDto>>.Failure($"Error retrieving goals: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}
