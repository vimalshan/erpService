using ObjectiveService.Domain.Entities;
using ObjectiveService.Domain.ValueObjects;

namespace ObjectiveService.Domain.Aggregates;

/// <summary>
/// Goal Aggregate Root — enforces Goal + SubGoal invariants.
/// </summary>
public class GoalAggregate
{
    private readonly Goal _goal;

    public decimal Id => _goal.Id;
    public string UserId => _goal.UserId;
    public string Status => _goal.Status;
    public IReadOnlyList<GoalSubGoal> SubGoals => _goal.SubGoals;

    public GoalAggregate(Goal goal) => _goal = goal ?? throw new ArgumentNullException(nameof(goal));

    public void AddSubGoal(string description, MeasurementRange measurement, string category)
    {
        var subGoal = new GoalSubGoal(
            _goal.Id, description,
            measurement.From, measurement.To,
            measurement.Unit, category);
        _goal.AddSubGoal(subGoal);
    }

    public void SubmitForApproval()
    {
        if (_goal.Status != "N")
            throw new InvalidOperationException($"Goal in status '{_goal.Status}' cannot be submitted.");
        _goal.SubmitForApproval(DateTime.UtcNow);
    }

    public void Approve() => _goal.ApproveGoal();

    public void Return(string remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
            throw new ArgumentException("Return remarks are required.", nameof(remarks));
        _goal.ReturnGoal(remarks);
    }

    public void Close() => _goal.CloseGoal();

    public Goal ToEntity() => _goal;
}

/// <summary>
/// Control Point Aggregate Root — enforces versioning and lifecycle invariants.
/// </summary>
public class ControlPointAggregate
{
    private readonly ControlPoint _controlPoint;

    public decimal Id => _controlPoint.Id;
    public string Status => _controlPoint.Status;
    public decimal VersionNumber => _controlPoint.VersionNumber;

    public ControlPointAggregate(ControlPoint controlPoint) =>
        _controlPoint = controlPoint ?? throw new ArgumentNullException(nameof(controlPoint));

    public void Modify(string description, MeasurementRange newMeasurement, decimal? weightage = null)
    {
        if (_controlPoint.Status == "D")
            throw new InvalidOperationException("Cannot modify a deleted control point.");

        _controlPoint.Update(description, newMeasurement.From, newMeasurement.To, weightage);
    }

    public void Delete()
    {
        if (_controlPoint.Status == "D")
            throw new InvalidOperationException("Control point is already deleted.");
        _controlPoint.Delete();
    }

    public ControlPoint ToEntity() => _controlPoint;
}
