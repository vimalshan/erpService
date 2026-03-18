using MediatR;
using Microsoft.AspNetCore.Routing;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.API.Endpoints;

/// <summary>
/// Minimal API endpoints for Insurance Management
/// </summary>
public static class InsuranceEndpoints
{
    public static void MapInsuranceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2")
            .WithName("Insurance Management Minimal APIs")
            .WithOpenApi();

        // Plans
        MapPlanEndpoints(group);

        // Enrollments  
        MapEnrollmentEndpoints(group);

        // Claims
        MapClaimEndpoints(group);
    }

    private static void MapPlanEndpoints(RouteGroupBuilder group)
    {
        var plansGroup = group.MapGroup("/plans")
            .WithName("Insurance Plans")
            .WithOpenApi();

        plansGroup.MapGet("/{id}", GetPlan)
            .WithName("GetPlanMinimal")
            .WithOpenApi();

        plansGroup.MapGet("/", GetAllPlans)
            .WithName("GetAllPlansMinimal")
            .WithOpenApi();

        plansGroup.MapPost("/", CreatePlan)
            .WithName("CreatePlanMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        plansGroup.MapPut("/{id}", UpdatePlan)
            .WithName("UpdatePlanMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        plansGroup.MapPatch("/{id}/deactivate", DeactivatePlan)
            .WithName("DeactivatePlanMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        plansGroup.MapPatch("/{id}/activate", ActivatePlan)
            .WithName("ActivatePlanMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();
    }

    private static void MapEnrollmentEndpoints(RouteGroupBuilder group)
    {
        var enrollmentsGroup = group.MapGroup("/enrollments")
            .WithName("Insurance Enrollments")
            .WithOpenApi();

        enrollmentsGroup.MapGet("/{id}", GetEnrollment)
            .WithName("GetEnrollmentMinimal")
            .WithOpenApi();

        enrollmentsGroup.MapGet("/employee/{empId}", GetEmployeeEnrollments)
            .WithName("GetEmployeeEnrollmentsMinimal")
            .WithOpenApi();

        enrollmentsGroup.MapGet("/employee/{empId}/active", GetActiveEnrollments)
            .WithName("GetActiveEnrollmentsMinimal")
            .WithOpenApi();

        enrollmentsGroup.MapPost("/", EnrollEmployee)
            .WithName("EnrollEmployeeMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        enrollmentsGroup.MapPatch("/{id}/terminate", TerminateEnrollment)
            .WithName("TerminateEnrollmentMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        enrollmentsGroup.MapPatch("/{id}/suspend", SuspendEnrollment)
            .WithName("SuspendEnrollmentMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();
    }

    private static void MapClaimEndpoints(RouteGroupBuilder group)
    {
        var claimsGroup = group.MapGroup("/claims")
            .WithName("Insurance Claims")
            .WithOpenApi();

        claimsGroup.MapGet("/{id}", GetClaim)
            .WithName("GetClaimMinimal")
            .WithOpenApi();

        claimsGroup.MapGet("/employee/{empId}", GetEmployeeClaims)
            .WithName("GetEmployeeClaimsMinimal")
            .WithOpenApi();

        claimsGroup.MapPost("/", SubmitClaim)
            .WithName("SubmitClaimMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        claimsGroup.MapPatch("/{id}/approve", ApproveClaim)
            .WithName("ApproveClaimMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        claimsGroup.MapPatch("/{id}/reject", RejectClaim)
            .WithName("RejectClaimMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();

        claimsGroup.MapPatch("/{id}/mark-paid", MarkPaid)
            .WithName("MarkPaidMinimal")
            .RequireAuthorization("Bearer")
            .WithOpenApi();
    }

    // Plan Handlers
    private static async Task<IResult> GetPlan(long id, IMediator mediator)
    {
        var query = new GetInsurancePlanByIdQuery { PlanId = id };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.NotFound(result);
    }

    private static async Task<IResult> GetAllPlans(IMediator mediator, int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetAllInsurancePlansQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreatePlan(CreateInsurancePlanDto dto, IMediator mediator, HttpContext context)
    {
        var command = new CreateInsurancePlanCommand
        {
            PlanName = dto.PlanName,
            PlanDescription = dto.PlanDescription,
            PremiumRate = dto.PremiumRate,
            MinPremium = dto.MinPremium,
            MaxPremium = dto.MaxPremium,
            CoverageDetails = dto.CoverageDetails,
            CreatedBy = GetUserId(context)
        };
        var result = await mediator.Send(command);
        return result.Success ? Results.Created($"/api/v2/plans/{result.Data?.InsurancePlanId}", result) : Results.BadRequest(result);
    }

    private static async Task<IResult> UpdatePlan(long id, UpdateInsurancePlanDto dto, IMediator mediator, HttpContext context)
    {
        var command = new UpdateInsurancePlanCommand
        {
            PlanId = id,
            PlanName = dto.PlanName,
            PlanDescription = dto.PlanDescription,
            PremiumRate = dto.PremiumRate,
            MinPremium = dto.MinPremium,
            MaxPremium = dto.MaxPremium,
            CoverageDetails = dto.CoverageDetails,
            ModifiedBy = GetUserId(context)
        };
        var result = await mediator.Send(command);
        return result.Success ? Results.Ok(result) : Results.NotFound(result);
    }

    private static async Task<IResult> DeactivatePlan(long id, IMediator mediator, HttpContext context)
    {
        var command = new DeactivateInsurancePlanCommand { PlanId = id, ModifiedBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> ActivatePlan(long id, IMediator mediator, HttpContext context)
    {
        var command = new ActivateInsurancePlanCommand { PlanId = id, ModifiedBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    // Enrollment Handlers
    private static async Task<IResult> GetEnrollment(long id, IMediator mediator)
    {
        var query = new GetInsuranceEnrollmentByIdQuery { EnrollmentId = id };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetEmployeeEnrollments(long empId, IMediator mediator)
    {
        var query = new GetEmployeeAllEnrollmentsQuery { EmpSysId = empId };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetActiveEnrollments(long empId, IMediator mediator)
    {
        var query = new GetEmployeeActiveEnrollmentsQuery { EmpSysId = empId };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> EnrollEmployee(CreateInsuranceEnrollmentDto dto, IMediator mediator, HttpContext context)
    {
        var command = new EnrollInsuranceCommand
        {
            EmpSysId = dto.EmpSysId,
            InsurancePlanId = dto.InsurancePlanId,
            CoverageType = dto.CoverageType,
            EnrollmentDate = dto.EnrollmentDate,
            EffectiveDate = dto.EffectiveDate,
            CreatedBy = GetUserId(context)
        };
        var result = await mediator.Send(command);
        return result.Success ? Results.Created($"/api/v2/enrollments/{result.Data?.EnrollmentId}", result) : Results.BadRequest(result);
    }

    private static async Task<IResult> TerminateEnrollment(long id, dynamic dto, IMediator mediator, HttpContext context)
    {
        var command = new TerminateEnrollmentCommand
        {
            EnrollmentId = id,
            Reason = dto?.reason ?? "N/A",
            ModifiedBy = GetUserId(context)
        };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> SuspendEnrollment(long id, IMediator mediator, HttpContext context)
    {
        var command = new SuspendEnrollmentCommand { EnrollmentId = id, ModifiedBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    // Claim Handlers
    private static async Task<IResult> GetClaim(long id, IMediator mediator)
    {
        var query = new GetInsuranceClaimByIdQuery { ClaimId = id };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetEmployeeClaims(long empId, IMediator mediator, int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetEmployeeClaimsQuery { EmpSysId = empId, PageNumber = pageNumber, PageSize = pageSize };
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> SubmitClaim(SubmitInsuranceClaimDto dto, IMediator mediator, HttpContext context)
    {
        var command = new SubmitClaimCommand
        {
            EmpSysId = GetEmpSysId(context),
            EnrollmentId = dto.EnrollmentId,
            ClaimType = dto.ClaimType,
            ClaimAmount = dto.ClaimAmount,
            ServiceDate = dto.ServiceDate,
            HospitalName = dto.HospitalName,
            Remarks = dto.Remarks,
            CreatedBy = GetUserId(context)
        };
        var result = await mediator.Send(command);
        return result.Success ? Results.Created($"/api/v2/claims/{result.Data?.ClaimId}", result) : Results.BadRequest(result);
    }

    private static async Task<IResult> ApproveClaim(long id, ApproveInsuranceClaimDto dto, IMediator mediator, HttpContext context)
    {
        var command = new ApproveClaimCommand { ClaimId = id, ApprovedAmount = dto.ApprovedAmount, ApprovedBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> RejectClaim(long id, RejectInsuranceClaimDto dto, IMediator mediator, HttpContext context)
    {
        var command = new RejectClaimCommand { ClaimId = id, RejectionReason = dto.RejectionReason, RejectedBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> MarkPaid(long id, IMediator mediator, HttpContext context)
    {
        var command = new MarkClaimAsPaidCommand { ClaimId = id, PaidBy = GetUserId(context) };
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static long GetUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst("sub") ?? context.User.FindFirst("user_id");
        return long.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
    }

    private static long GetEmpSysId(HttpContext context)
    {
        var empIdClaim = context.User.FindFirst("emp_sys_id") ?? context.User.FindFirst("employee_id");
        return long.TryParse(empIdClaim?.Value, out var empId) ? empId : 0;
    }
}
