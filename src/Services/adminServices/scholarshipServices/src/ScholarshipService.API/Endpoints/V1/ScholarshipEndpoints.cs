using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScholarshipService.Application.Commands.ApproveScholarship;
using ScholarshipService.Application.Commands.CreateScholarship;
using ScholarshipService.Application.Commands.StopScholarship;
using ScholarshipService.Application.Common;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Application.Queries.GetScholarshipById;
using ScholarshipService.Application.Queries.GetScholarships;

namespace ScholarshipService.API.Endpoints.V1;

public static class ScholarshipEndpoints
{
    public static IEndpointRouteBuilder MapScholarshipEndpoints(this IEndpointRouteBuilder app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/scholarships")
            .WithTags("Scholarships")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1, 0)
            .RequireAuthorization();

        group.MapGet("/", GetScholarships)
            .WithName("GetScholarships")
            .WithSummary("Get all scholarships (paged)");

        group.MapGet("/{id:int}", GetById)
            .WithName("GetScholarshipById")
            .WithSummary("Get scholarship by ID");

        group.MapGet("/employee/{employeeId:int}", GetByEmployee)
            .WithName("GetScholarshipsByEmployee")
            .WithSummary("Get scholarships for an employee");

        group.MapPost("/", Create)
            .WithName("CreateScholarship")
            .WithSummary("Submit a new scholarship application");

        group.MapPut("/{id:int}/approve", Approve)
            .WithName("ApproveScholarship")
            .WithSummary("Approve a scholarship application");

        group.MapPut("/{id:int}/stop", Stop)
            .WithName("StopScholarship")
            .WithSummary("Stop an active scholarship");

        return app;
    }

    private static async Task<IResult> GetScholarships(
        IMediator mediator, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetScholarshipsQuery(null, page, pageSize), ct);
        return Results.Ok(BaseResponse<PagedResult<ScholarshipMainDto>>.Ok(result));
    }

    private static async Task<IResult> GetById(IMediator mediator, int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetScholarshipByIdQuery(id), ct);
        return result is null
            ? Results.NotFound(BaseResponse<ScholarshipMainDto>.Fail($"Scholarship {id} not found."))
            : Results.Ok(BaseResponse<ScholarshipMainDto>.Ok(result));
    }

    private static async Task<IResult> GetByEmployee(IMediator mediator, int employeeId,
        int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetScholarshipsQuery(employeeId, page, pageSize), ct);
        return Results.Ok(BaseResponse<PagedResult<ScholarshipMainDto>>.Ok(result));
    }

    private static async Task<IResult> Create(
        IMediator mediator, [FromBody] CreateScholarshipCommand command, CancellationToken ct = default)
    {
        var id = await mediator.Send(command, ct);
        return Results.Created($"/api/v1/scholarships/{id}",
            BaseResponse<int>.Ok(id, "Scholarship application submitted successfully."));
    }

    private static async Task<IResult> Approve(
        IMediator mediator, int id, [FromBody] ApproveScholarshipRequest request, CancellationToken ct = default)
    {
        await mediator.Send(new ApproveScholarshipCommand(id, request.ApprovedBy, request.Remarks), ct);
        return Results.Ok(BaseResponse<bool>.Ok(true, "Scholarship approved successfully."));
    }

    private static async Task<IResult> Stop(
        IMediator mediator, int id, [FromBody] StopScholarshipRequest request, CancellationToken ct = default)
    {
        await mediator.Send(new StopScholarshipCommand(id, request.Reason, request.StoppedBy), ct);
        return Results.Ok(BaseResponse<bool>.Ok(true, "Scholarship stopped successfully."));
    }
}

public record ApproveScholarshipRequest(int ApprovedBy, string? Remarks = null);
public record StopScholarshipRequest(string Reason, int StoppedBy);
