using DeductionService.Application.CQRS.Commands.CreateAdhocDeduction;
using DeductionService.Application.CQRS.Queries.GetDeductionsByEmployee;
using DeductionService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DeductionService.API.MinimalApis;

public static class DeductionEndpoints
{
    public static IEndpointRouteBuilder MapDeductionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/deductions")
            .WithTags("Deductions (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/employee/{employeeNumber:long}", async (
            long employeeNumber,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDeductionsByEmployeeQuery(employeeNumber), ct);
            return Results.Ok(result);
        })
        .WithName("GetDeductionsByEmployeeV2")
        .WithSummary("Get all deductions for an employee (Minimal API)")
        .Produces<IEnumerable<AdhocPayDeductionDto>>();

        group.MapPost("/", async (
            CreateAdhocDeductionDto dto,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateAdhocDeductionCommand(
                dto.SystemId, dto.CanteenUnit, dto.PayAmount,
                dto.EarningDeductionCode, dto.EmployeeNumber,
                dto.EnteredByUserId, dto.CompanyCode, dto.GradeType), ct);

            return Results.Created($"/api/v2/deductions/{result.SystemId}", result);
        })
        .WithName("CreateDeductionV2")
        .WithSummary("Create an ad-hoc deduction (Minimal API)")
        .Produces<AdhocPayDeductionDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        return app;
    }
}
