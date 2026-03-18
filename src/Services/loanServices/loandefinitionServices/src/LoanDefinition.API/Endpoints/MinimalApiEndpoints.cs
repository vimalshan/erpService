using LoanDefinition.Application.Features.Loans.Queries;
using LoanDefinition.Application.Features.LoanTypes.Queries;
using LoanDefinition.Infrastructure.Dapper;
using MediatR;

namespace LoanDefinition.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2").WithTags("Minimal API");

        group.MapGet("/loan-types", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllLoanTypesQuery())))
            .WithName("GetAllLoanTypesV2");

        group.MapGet("/loan-types/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLoanTypeByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetLoanTypeByIdV2");

        group.MapGet("/loans", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllLoansQuery())))
            .WithName("GetAllLoansV2");

        group.MapGet("/loans/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLoanByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetLoanByIdV2");

        group.MapGet("/loans/{id:long}/details", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLoanDetailQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetLoanDetailsV2");

        group.MapGet("/loans/active", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetActiveLoansQuery())))
            .WithName("GetActiveLoansV2");

        group.MapGet("/loans/{id:long}/interest-rates", async (long id, ILoanDapperQueries dapper) =>
            Results.Ok(await dapper.GetInterestRatesByLoanIdAsync(id)))
            .WithName("GetInterestRatesV2");

        group.MapGet("/loans/{id:long}/limit-ranges", async (long id, ILoanDapperQueries dapper) =>
            Results.Ok(await dapper.GetLimitRangesByLoanIdAsync(id)))
            .WithName("GetLimitRangesV2");

        return app;
    }
}
