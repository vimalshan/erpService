using Microsoft.AspNetCore.OpenApi;

namespace CompensationService.API.Endpoints;

/// <summary>
/// Minimal APIs for Compensation Grades (Alternative to Controllers)
/// Can be used instead of or alongside REST controllers
/// </summary>
public static class CompensationGradeEndpoints
{
    public static void MapCompensationGradeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/grades")
            .WithName("CompensationGrades")
            .WithOpenApi();

        group.MapGet("/", GetAllGrades)
            .WithName("GetAllGrades")
            .WithDescription("Get all compensation grades");

        group.MapGet("/{id}", GetGradeById)
            .WithName("GetGradeById")
            .WithDescription("Get compensation grade by ID");

        group.MapPost("/", CreateGrade)
            .WithName("CreateGrade")
            .WithDescription("Create new compensation grade");

        group.MapPut("/{id}", UpdateGrade)
            .WithName("UpdateGrade")
            .WithDescription("Update compensation grade");
    }

    private static async Task<IResult> GetAllGrades(IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetAllCompensationGradesQuery();
        var grades = await mediator.Send(query, cancellationToken);
        return Results.Ok(grades);
    }

    private static async Task<IResult> GetGradeById(long id, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetCompensationGradeByIdQuery { GradeId = id };
            var grade = await mediator.Send(query, cancellationToken);
            return Results.Ok(grade);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> CreateGrade(
        CreateCompensationGradeDto dto,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateCompensationGradeCommand
            {
                GradeCode = dto.GradeCode,
                GradeName = dto.GradeName,
                GradeLevel = dto.GradeLevel,
                BaseSalary = dto.BaseSalary,
                HraPercentage = dto.HraPercentage ?? 0,
                DaPercentage = dto.DaPercentage ?? 0,
                EffectiveFrom = dto.EffectiveFrom,
                CreatedBy = 1
            };

            var result = await mediator.Send(command, cancellationToken);
            return Results.CreatedAtRoute("GetGradeById", new { id = result.GradeId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> UpdateGrade(
        long id,
        UpdateCompensationGradeDto dto,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            dto.GradeId = id;
            var command = new UpdateCompensationGradeCommand
            {
                GradeId = dto.GradeId,
                GradeName = dto.GradeName,
                BaseSalary = dto.BaseSalary,
                HraPercentage = dto.HraPercentage ?? 0,
                DaPercentage = dto.DaPercentage ?? 0,
                UpdatedBy = 1
            };

            var result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}
