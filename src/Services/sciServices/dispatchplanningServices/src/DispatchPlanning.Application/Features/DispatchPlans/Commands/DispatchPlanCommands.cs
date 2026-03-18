using DispatchPlanning.Application.DTOs;
using MediatR;

namespace DispatchPlanning.Application.Features.DispatchPlans.Commands;

public record CreateDispatchPlanCommand(
    char PlanType,
    DateTime PlanMonth,
    int CompanyUnitId,
    int ModifiedBy,
    string? MPlus1,
    string? MPlus2,
    string? MPlus3,
    string? MPlus4
) : IRequest<int>;

public record AddDispatchPlanItemCommand(
    int PlanHeaderId,
    int BreakupItemId,
    long? TargetWeek1,
    long? TargetWeek2,
    long? TargetWeek3,
    long? TargetWeek4,
    long? TargetWeek5,
    long? TargetMPlus1,
    long? TargetMPlus2,
    long? TargetMPlus3,
    long? TargetMPlus4,
    int ModifiedBy
) : IRequest<Unit>;

public record UpdateDispatchPlanForecastCommand(
    int PlanHeaderId,
    string? MPlus1,
    string? MPlus2,
    string? MPlus3,
    string? MPlus4,
    int ModifiedBy
) : IRequest<Unit>;

public record DeleteDispatchPlanCommand(int PlanHeaderId, int DeletedBy) : IRequest<Unit>;

public record AddSubGroupTargetCommand(
    int PlanHeaderId,
    int SubGroupId,
    long? TargetWeek1,
    long? TargetWeek2,
    long? TargetWeek3,
    long? TargetWeek4,
    long? TargetWeek5,
    long? TargetMPlus1,
    long? TargetMPlus2,
    long? TargetMPlus3,
    long? TargetMPlus4,
    int ModifiedBy
) : IRequest<Unit>;
