using AgencyService.Application.Commands;
using AgencyService.Application.DTOs;
using AgencyService.Application.Queries;
using MediatR;

namespace AgencyService.Api.Endpoints;

public static class AgencyEndpoints
{
    public static void MapAgencyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/agencies")
            .WithName("Agencies");
        
        group.MapGet("/", GetAllAgencies)
            .WithName("GetAllAgencies");
        
        group.MapGet("/{agencyCode}", GetAgency)
            .WithName("GetAgency");
        
        group.MapGet("/type/{type}", GetAgenciesByType)
            .WithName("GetAgenciesByType");
        
        group.MapPost("/", CreateAgency)
            .WithName("CreateAgency");
        
        group.MapPut("/{agencyCode}", UpdateAgency)
            .WithName("UpdateAgency");
        
        group.MapDelete("/{agencyCode}", DeleteAgency)
            .WithName("DeleteAgency");
    }
    
    private static async Task<IResult> GetAllAgencies(IMediator mediator)
    {
        var query = new GetAllAgenciesQuery();
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> GetAgency(long agencyCode, IMediator mediator)
    {
        var query = new GetAgencyByCodeQuery { AgencyCode = agencyCode };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.NotFound(result);
    }
    
    private static async Task<IResult> GetAgenciesByType(string type, IMediator mediator)
    {
        var query = new GetAgenciesByTypeQuery { Type = type };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> CreateAgency(CreateAgencyCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Success 
            ? Results.CreatedAtRoute("GetAgency", new { agencyCode = result.Data }, result)
            : Results.BadRequest(result);
    }
    
    private static async Task<IResult> UpdateAgency(
        long agencyCode,
        UpdateAgencyRequest request,
        IMediator mediator)
    {
        var command = new UpdateAgencyCommand
        {
            AgencyCode = agencyCode,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };
        
        var result = await mediator.Send(command);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> DeleteAgency(long agencyCode, IMediator mediator)
    {
        var command = new DeleteAgencyCommand { AgencyCode = agencyCode };
        var result = await mediator.Send(command);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
}

public class UpdateAgencyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public static class VendorEndpoints
{
    public static void MapVendorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/vendors")
            .WithName("Vendors");
        
        group.MapGet("/", GetAllVendors)
            .WithName("GetAllVendors");
        
        group.MapGet("/{vendorId}", GetVendor)
            .WithName("GetVendor");
        
        group.MapGet("/category/{categoryType}", GetVendorsByCategory)
            .WithName("GetVendorsByCategory");
        
        group.MapPost("/", CreateVendor)
            .WithName("CreateVendor");
        
        group.MapPut("/{vendorId}", UpdateVendor)
            .WithName("UpdateVendor");
        
        group.MapDelete("/{vendorId}", DeleteVendor)
            .WithName("DeleteVendor");
    }
    
    private static async Task<IResult> GetAllVendors(IMediator mediator)
    {
        var query = new GetAllVendorsQuery();
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> GetVendor(long vendorId, IMediator mediator)
    {
        var query = new GetVendorByIdQuery { VendorId = vendorId };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.NotFound(result);
    }
    
    private static async Task<IResult> GetVendorsByCategory(string categoryType, IMediator mediator)
    {
        var query = new GetVendorsByCategoryQuery { CategoryType = categoryType };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> CreateVendor(CreateVendorCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Success 
            ? Results.CreatedAtRoute("GetVendor", new { vendorId = result.Data }, result)
            : Results.BadRequest(result);
    }
    
    private static async Task<IResult> UpdateVendor(
        long vendorId,
        UpdateVendorRequest request,
        IMediator mediator)
    {
        var command = new UpdateVendorCommand
        {
            VendorId = vendorId,
            Name = request.Name,
            Phone = request.Phone,
            CityCode = request.CityCode
        };
        
        var result = await mediator.Send(command);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> DeleteVendor(long vendorId, IMediator mediator)
    {
        var command = new DeleteVendorCommand { VendorId = vendorId };
        var result = await mediator.Send(command);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
}

public class UpdateVendorRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public long? CityCode { get; set; }
}

public static class AirlineEndpoints
{
    public static void MapAirlineEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/airlines")
            .WithName("Airlines");
        
        group.MapGet("/", GetAllAirlines)
            .WithName("GetAllAirlines");
        
        group.MapGet("/{code}", GetAirline)
            .WithName("GetAirline");
        
        group.MapPost("/", CreateAirline)
            .WithName("CreateAirline");
    }
    
    private static async Task<IResult> GetAllAirlines(IMediator mediator)
    {
        var query = new GetAllAirlinesQuery();
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private static async Task<IResult> GetAirline(string code, IMediator mediator)
    {
        var query = new GetAirlineByCodeQuery { Code = code };
        var result = await mediator.Send(query);
        return result.Success ? Results.Ok(result) : Results.NotFound(result);
    }
    
    private static async Task<IResult> CreateAirline(CreateAirlineCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Success 
            ? Results.CreatedAtRoute("GetAirline", new { code = result.Data }, result)
            : Results.BadRequest(result);
    }
}
