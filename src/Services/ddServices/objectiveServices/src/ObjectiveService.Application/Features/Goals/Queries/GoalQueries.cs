using ObjectiveService.Application.Common;
using ObjectiveService.Application.DTOs;

namespace ObjectiveService.Application.Features.Goals.Queries;

public class GetGoalByIdQuery : QueryBase<CommandResult<GoalDto>>
{
    public decimal Id { get; set; }

    public GetGoalByIdQuery(decimal id)
    {
        Id = id;
    }
}

public class GetGoalsByEmployeeQuery : QueryBase<CommandResult<List<GoalDto>>>
{
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }

    public GetGoalsByEmployeeQuery(string userId, decimal pinNumber)
    {
        UserId = userId;
        PinNumber = pinNumber;
    }
}

public class GetGoalsByPeriodQuery : QueryBase<CommandResult<List<GoalDto>>>
{
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetActiveDraftGoalsQuery : QueryBase<CommandResult<List<GoalDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
