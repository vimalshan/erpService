using Asp.Versioning;
using MediatR;
using ScholarshipService.Application.Common;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Application.Queries.GetScholarshipAmounts;

namespace ScholarshipService.API.Endpoints.V1;

public static class ScholarshipAmountEndpoints
{
    public static IEndpointRouteBuilder MapScholarshipAmountEndpoints(this IEndpointRouteBuilder app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/scholarship-amounts")
            .WithTags("ScholarshipAmounts")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1, 0)
            .RequireAuthorization();

        group.MapGet("/", GetAll)
            .WithName("GetScholarshipAmounts")
            .WithSummary("Get all scholarship amount configurations");

        return app;
    }

    private static async Task<IResult> GetAll(IMediator mediator, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetScholarshipAmountsQuery(), ct);
        return Results.Ok(BaseResponse<IEnumerable<ScholarshipAmountDto>>.Ok(result));
    }
}
