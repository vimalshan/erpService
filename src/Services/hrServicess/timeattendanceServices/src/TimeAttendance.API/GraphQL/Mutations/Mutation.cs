using MediatR;
using TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Commands.DeleteAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Commands.UpdateAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismMis.Commands.CreateAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Commands.DeleteAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Commands.UpdateAbsenteeismMis;

namespace TimeAttendance.API.GraphQL.Mutations;

public record CreateAbsenteeismDetailInput(
    long UnitId,
    int Year,
    int Month,
    long TotalManDays,
    long AbsentManDays,
    string GradeCategory,
    long FunctionId,
    long AgeId,
    long ExperienceId,
    string Gender,
    long InternalExperienceId,
    long TotalExperienceId
);

public class Mutation
{
    [GraphQLDescription("Create a new absenteeism detail record.")]
    public async Task<long> CreateAbsenteeismDetail(
        [Service] IMediator mediator,
        CreateAbsenteeismDetailInput input,
        CancellationToken cancellationToken = default)
    {
        var gender = string.IsNullOrWhiteSpace(input.Gender)
            ? 'U'
            : char.ToUpperInvariant(input.Gender[0]);

        var command = new CreateAbsenteeismDetailCommand(
            input.UnitId,
            input.Year,
            input.Month,
            input.TotalManDays,
            input.AbsentManDays,
            input.GradeCategory,
            input.FunctionId,
            input.AgeId,
            input.ExperienceId,
            gender,
            input.InternalExperienceId,
            input.TotalExperienceId);

        return await mediator.Send(command, cancellationToken);
    }

    [GraphQLDescription("Update an absenteeism detail record.")]
    public async Task<bool> UpdateAbsenteeismDetail(
        [Service] IMediator mediator,
        UpdateAbsenteeismDetailCommand input,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Delete an absenteeism detail record.")]
    public async Task<bool> DeleteAbsenteeismDetail(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new DeleteAbsenteeismDetailCommand(id), cancellationToken);

    [GraphQLDescription("Create a new absenteeism MIS record.")]
    public async Task<long> CreateAbsenteeismMis(
        [Service] IMediator mediator,
        CreateAbsenteeismMisCommand input,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Update an absenteeism MIS record.")]
    public async Task<bool> UpdateAbsenteeismMis(
        [Service] IMediator mediator,
        UpdateAbsenteeismMisCommand input,
        CancellationToken cancellationToken = default)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Delete an absenteeism MIS record.")]
    public async Task<bool> DeleteAbsenteeismMis(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new DeleteAbsenteeismMisCommand(id), cancellationToken);
}
