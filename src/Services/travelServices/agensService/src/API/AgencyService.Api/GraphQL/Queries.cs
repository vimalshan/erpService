using AgencyService.Application.DTOs;
using AgencyService.Application.Queries;
using AgencyService.Application.Commands;
using MediatR;

namespace AgencyService.Api.GraphQL;

public class Query
{
    public async Task<AgencyDto?> GetAgency(long agencyCode, IMediator mediator)
    {
        var query = new GetAgencyByCodeQuery { AgencyCode = agencyCode };
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
    
    public async Task<List<AgencyDto>?> GetAllAgencies(IMediator mediator)
    {
        var query = new GetAllAgenciesQuery();
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
    
    public async Task<List<VendorDto>?> GetAllVendors(IMediator mediator)
    {
        var query = new GetAllVendorsQuery();
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
    
    public async Task<VendorDto?> GetVendor(long vendorId, IMediator mediator)
    {
        var query = new GetVendorByIdQuery { VendorId = vendorId };
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
    
    public async Task<List<AirlineDto>?> GetAllAirlines(IMediator mediator)
    {
        var query = new GetAllAirlinesQuery();
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
    
    public async Task<AirlineDto?> GetAirline(string code, IMediator mediator)
    {
        var query = new GetAirlineByCodeQuery { Code = code };
        var result = await mediator.Send(query);
        return result.Success ? result.Data : null;
    }
}

public class Mutation
{
    public async Task<AgencyDto?> CreateAgency(
        long agencyCode,
        string name,
        string type,
        string email,
        string phone,
        string address1,
        IMediator mediator)
    {
        var command = new Application.Commands.CreateAgencyCommand
        {
            AgencyCode = agencyCode,
            Name = name,
            Type = type,
            Email = email,
            Phone = phone,
            Address1 = address1
        };
        
        var result = await mediator.Send(command);
        if (result.Success)
        {
            var query = new GetAgencyByCodeQuery { AgencyCode = agencyCode };
            var queryResult = await mediator.Send(query);
            return queryResult.Success ? queryResult.Data : null;
        }
        return null;
    }
    
    public async Task<VendorDto?> CreateVendor(
        long vendorId,
        string name,
        string categoryType,
        IMediator mediator)
    {
        var command = new CreateVendorCommand
        {
            VendorId = vendorId,
            Name = name,
            CategoryType = categoryType
        };
        
        var result = await mediator.Send(command);
        if (result.Success)
        {
            var query = new GetVendorByIdQuery { VendorId = vendorId };
            var queryResult = await mediator.Send(query);
            return queryResult.Success ? queryResult.Data : null;
        }
        return null;
    }
}
