using ConfigService.Application.DTOs;
using ConfigService.Application.Features.Currencies.Queries;
using ConfigService.Application.Features.Travel.Queries;
using ConfigService.Application.Features.Vendors.Queries;
using MediatR;

namespace ConfigService.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal").RequireAuthorization();

        group.MapGet("/currencies", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCurrenciesQuery(), ct)))
            .WithName("GetCurrenciesMinimal")
            .WithTags("Currencies");

        group.MapGet("/currencies/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCurrencyByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetCurrencyByIdMinimal")
        .WithTags("Currencies");

        group.MapGet("/countries", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCountriesQuery(), ct)))
            .WithName("GetCountriesMinimal")
            .WithTags("Travel");

        group.MapGet("/cities", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCitiesQuery(), ct)))
            .WithName("GetCitiesMinimal")
            .WithTags("Travel");

        group.MapGet("/cities/country/{countryId}", async (string countryId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetCitiesByCountryQuery(countryId), ct)))
            .WithName("GetCitiesByCountryMinimal")
            .WithTags("Travel");

        group.MapGet("/vendors", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllVendorsQuery(), ct)))
            .WithName("GetVendorsMinimal")
            .WithTags("Vendors");

        group.MapGet("/vendors/active", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetActiveVendorsQuery(), ct)))
            .WithName("GetActiveVendorsMinimal")
            .WithTags("Vendors");

        group.MapGet("/travel-classes", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllTravelClassesQuery(), ct)))
            .WithName("GetTravelClassesMinimal")
            .WithTags("Travel");

        group.MapGet("/travel-contacts", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllTravelContactsQuery(), ct)))
            .WithName("GetTravelContactsMinimal")
            .WithTags("Travel");

        return app;
    }
}
