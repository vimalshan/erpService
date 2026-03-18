using AutoMapper;
using MediatR;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.Features.Goals.Commands;
using ObjectiveService.Application.Interfaces;
using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.Goals.Handlers;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, CommandResult<decimal>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult<decimal>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var goal = new Goal(
                request.UserId,
                request.PinNumber,
                request.PeriodFrom,
                request.PeriodTo,
                request.ReferenceNumber,
                request.FormFlag
            );

            foreach (var subGoalItem in request.SubGoals)
            {
                var subGoal = new GoalSubGoal(
                    goal.Id,
                    subGoalItem.Description,
                    subGoalItem.UnitFrom,
                    subGoalItem.UnitTo,
                    subGoalItem.UnitOfMeasurement,
                    subGoalItem.Category
                );
                goal.AddSubGoal(subGoal);
            }

            var repository = _unitOfWork.Repository<Goal>();
            await repository.AddAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult<decimal>.Success(goal.Id, "Goal created successfully");
        }
        catch (Exception ex)
        {
            return CommandResult<decimal>.Failure($"Error creating goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class SubmitGoalForApprovalCommandHandler : IRequestHandler<SubmitGoalForApprovalCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitGoalForApprovalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(SubmitGoalForApprovalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goal = await repository.GetByIdAsync(request.GoalId, cancellationToken);

            if (goal == null)
                return CommandResult.Failure("Goal not found");

            goal.SubmitForApproval(DateTime.UtcNow);

            await repository.UpdateAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Goal submitted for approval successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error submitting goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class ApproveGoalCommandHandler : IRequestHandler<ApproveGoalCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(ApproveGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goal = await repository.GetByIdAsync(request.GoalId, cancellationToken);

            if (goal == null)
                return CommandResult.Failure("Goal not found");

            goal.ApproveGoal();

            await repository.UpdateAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Goal approved successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error approving goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class ReturnGoalCommandHandler : IRequestHandler<ReturnGoalCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReturnGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(ReturnGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goal = await repository.GetByIdAsync(request.GoalId, cancellationToken);

            if (goal == null)
                return CommandResult.Failure("Goal not found");

            goal.ReturnGoal(request.Remarks);

            await repository.UpdateAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Goal returned for revision successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error returning goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class CloseGoalCommandHandler : IRequestHandler<CloseGoalCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CloseGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(CloseGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<Goal>();
            var goal = await repository.GetByIdAsync(request.GoalId, cancellationToken);

            if (goal == null)
                return CommandResult.Failure("Goal not found");

            goal.CloseGoal();

            await repository.UpdateAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Goal closed successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error closing goal: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}

public class RecordGoalAchievementCommandHandler : IRequestHandler<RecordGoalAchievementCommand, CommandResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RecordGoalAchievementCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResult> Handle(RecordGoalAchievementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<GoalSubGoal>();
            var subGoal = await repository.GetByIdAsync(request.GoalSubGoalId, cancellationToken);

            if (subGoal == null)
                return CommandResult.Failure("Goal Sub Goal not found");

            subGoal.RecordAchievement(request.Achievement, request.Difference);

            await repository.UpdateAsync(subGoal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CommandResult.Success("Goal achievement recorded successfully");
        }
        catch (Exception ex)
        {
            return CommandResult.Failure($"Error recording achievement: {ex.Message}", new List<string> { ex.InnerException?.Message });
        }
    }
}
