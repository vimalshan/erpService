using CompensationService.Application.DTOs;
using CompensationService.Application.Commands;
using CompensationService.Application.Queries;
using MediatR;

namespace CompensationService.API.GraphQL;

/// <summary>
/// GraphQL Query type for compensation grades
/// </summary>
public class CompensationGradeQuery
{
    [GraphQLName("compensationGrades")]
    public async Task<IEnumerable<CompensationGradeDto>> GetAllGradesAsync([Service] IMediator mediator)
    {
        var query = new GetAllCompensationGradesQuery();
        return await mediator.Send(query);
    }

    [GraphQLName("activeCompensationGrades")]
    public async Task<IEnumerable<CompensationGradeDto>> GetActiveGradesAsync([Service] IMediator mediator)
    {
        var query = new GetActiveCompensationGradesQuery();
        return await mediator.Send(query);
    }

    [GraphQLName("compensationGrade")]
    public async Task<CompensationGradeDto?> GetGradeByIdAsync([Service] IMediator mediator, long id)
    {
        try
        {
            var query = new GetCompensationGradeByIdQuery { GradeId = id };
            return await mediator.Send(query);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
}

/// <summary>
/// GraphQL Mutation type for compensation grades
/// </summary>
public class CompensationGradeMutation
{
    [GraphQLName("createCompensationGrade")]
    public async Task<CompensationGradeDto> CreateGradeAsync(
        [Service] IMediator mediator,
        string gradeCode,
        string gradeName,
        int gradeLevel,
        decimal baseSalary,
        decimal hraPercentage,
        decimal daPercentage,
        DateTime effectiveFrom)
    {
        var command = new CreateCompensationGradeCommand
        {
            GradeCode = gradeCode,
            GradeName = gradeName,
            GradeLevel = gradeLevel,
            BaseSalary = baseSalary,
            HraPercentage = hraPercentage,
            DaPercentage = daPercentage,
            EffectiveFrom = effectiveFrom,
            CreatedBy = 1
        };

        return await mediator.Send(command);
    }

    [GraphQLName("updateCompensationGrade")]
    public async Task<CompensationGradeDto> UpdateGradeAsync(
        [Service] IMediator mediator,
        long gradeId,
        string gradeName,
        decimal baseSalary,
        decimal hraPercentage,
        decimal daPercentage)
    {
        var command = new UpdateCompensationGradeCommand
        {
            GradeId = gradeId,
            GradeName = gradeName,
            BaseSalary = baseSalary,
            HraPercentage = hraPercentage,
            DaPercentage = daPercentage,
            UpdatedBy = 1
        };

        return await mediator.Send(command);
    }

    [GraphQLName("changeCompensationGradeStatus")]
    public async Task<bool> ChangeStatusAsync(
        [Service] IMediator mediator,
        long gradeId,
        string newStatus)
    {
        var command = new ChangeCompensationGradeStatusCommand
        {
            GradeId = gradeId,
            NewStatus = newStatus[0],
            ChangedBy = 1
        };

        return await mediator.Send(command);
    }
}
