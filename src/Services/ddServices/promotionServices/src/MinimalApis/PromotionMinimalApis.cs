using MediatR;
using PromotionService.DTOs;
using PromotionService.Features.Commands;
using PromotionService.Features.Queries;
using Microsoft.AspNetCore.Mvc;

namespace PromotionService.MinimalApis;

/// <summary>Minimal API endpoints — lightweight alternative to controller-based endpoints.</summary>
public static class PromotionMinimalApis
{
    public static IEndpointRouteBuilder MapPromotionMinimalApis(this IEndpointRouteBuilder app)
    {
        var grp = app.MapGroup("/api/v1/minimal/promotions")
            .WithTags("Promotion Minimal APIs")
            .RequireAuthorization();

        // ── Health ping ─────────────────────────────────────────
        grp.MapGet("/ping", () => Results.Ok(new { Status = "Healthy", Service = "Promotion Service", Time = DateTime.UtcNow }))
            .WithName("PromotionPing")
            .AllowAnonymous();

        // ── Ratings ─────────────────────────────────────────────
        grp.MapGet("/ratings/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetRatingByIdQuery { RatingId = id }, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        }).WithName("GetRatingMinimal");

        grp.MapGet("/ratings", async (
            [FromQuery] int? ddYear, [FromQuery] string? status,
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllRatingsQuery
            {
                DDYear = ddYear, Status = status,
                PageNumber = pageNumber <= 0 ? 1 : pageNumber,
                PageSize = pageSize <= 0 ? 10 : pageSize
            }, ct);
            return Results.Ok(result);
        }).WithName("GetAllRatingsMinimal");

        grp.MapPost("/ratings", async (CreateRatingDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateRatingCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                DDYear = dto.DDYear,
                AppraisalScore = dto.AppraisalScore,
                CompetencyScore = dto.CompetencyScore,
                GoalCompletionScore = dto.GoalCompletionScore
            }, ct);
            return Results.Created($"/api/v1/minimal/promotions/ratings/{result.RatingId}", result);
        }).WithName("CreateRatingMinimal").RequireAuthorization("HR,Admin");

        // ── Promotion Recommendations ────────────────────────────
        grp.MapGet("/recommendations/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetPromotionByIdQuery { PromotionId = id }, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        }).WithName("GetPromotionMinimal");

        grp.MapGet("/recommendations/pending", async (
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingPromotionsQuery
            { PageNumber = pageNumber <= 0 ? 1 : pageNumber, PageSize = pageSize <= 0 ? 10 : pageSize }, ct);
            return Results.Ok(result);
        }).WithName("GetPendingPromotionsMinimal").RequireAuthorization("HR,Admin");

        grp.MapPost("/recommendations/{id:long}/approve", async (long id, ApprovePromotionDto dto, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new ApprovePromotionRecommendationCommand { PromotionId = id, ApprovedBySystemId = dto.ApprovedBySystemId }, ct);
            return Results.Ok();
        }).WithName("ApprovePromotionMinimal").RequireAuthorization("Admin");

        // ── Horizontal Promotions ────────────────────────────────
        grp.MapGet("/horizontal/{transId:decimal}", async (decimal transId, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetHorizontalPromotionByIdQuery { TransactionId = transId }, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        }).WithName("GetHorizontalPromotionMinimal");

        grp.MapPost("/horizontal", async (CreateHorizontalPromotionDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateHorizontalPromotionCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                PromotionScore = dto.PromotionScore,
                GradeId = dto.GradeId,
                CurrentLevelId = dto.CurrentLevelId,
                NewLevelId = dto.NewLevelId,
                EffectiveFrom = dto.EffectiveFrom,
                PositionId = dto.PositionId,
                OldPositionName = dto.OldPositionName,
                OldPositionDesignation = dto.OldPositionDesignation,
                NewPositionName = dto.NewPositionName,
                NewPositionDesignation = dto.NewPositionDesignation,
                UpdatedBy = 0
            }, ct);
            return Results.Created($"/api/v1/minimal/promotions/horizontal/{result.TransactionId}", result);
        }).WithName("CreateHorizontalPromotionMinimal").RequireAuthorization("HR,Admin");

        // ── VTC Corrections ──────────────────────────────────────
        grp.MapGet("/vtccorrections/pending", async (
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingVTCCorrectionsQuery
            { PageNumber = pageNumber <= 0 ? 1 : pageNumber, PageSize = pageSize <= 0 ? 10 : pageSize }, ct);
            return Results.Ok(result);
        }).WithName("GetPendingVTCCorrectionsMinimal").RequireAuthorization("HR,Admin");

        grp.MapPost("/vtccorrections/{rateId:decimal}/approve", async (decimal rateId, ApproveVTCCorrectionDto dto, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new ApproveVTCCorrectionCommand { RateId = rateId, ApprovedBy = dto.ApprovedBy }, ct);
            return Results.Ok();
        }).WithName("ApproveVTCCorrectionMinimal").RequireAuthorization("Admin");

        // ── Appraisal Amounts ────────────────────────────────────
        grp.MapGet("/appraisalamounts", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllAppraisalAmountsQuery
            { PageNumber = pageNumber <= 0 ? 1 : pageNumber, PageSize = pageSize <= 0 ? 10 : pageSize }, ct);
            return Results.Ok(result);
        }).WithName("GetAppraisalAmountsMinimal");

        return app;
    }
}
