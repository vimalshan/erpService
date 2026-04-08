using HotChocolate;
using MediatR;
using MasterData.Application.Commands.CompanyUnit;
using MasterData.Application.Commands.Location;
using MasterData.Application.Commands.Supplier;
using MasterData.Application.Commands.State;
using MasterData.Application.Commands.City;
using MasterData.Application.Queries.CompanyUnit;
using MasterData.Application.Queries.Location;
using MasterData.Application.Queries.Supplier;
using MasterData.Application.Queries.State;
using MasterData.Application.Queries.City;
using MasterData.Application.DTOs;

namespace MasterData.API.GraphQL
{
    public class Query
    {
        public async Task<IReadOnlyList<CompanyUnitDto>> GetCompanyUnits(IMediator mediator)
        {
            return await mediator.Send(new GetAllCompanyUnitsQuery());
        }

        public async Task<CompanyUnitDto?> GetCompanyUnit(int id, IMediator mediator)
        {
            return await mediator.Send(new GetCompanyUnitByIdQuery(id));
        }

        public async Task<IReadOnlyList<LocationDto>> GetLocations(IMediator mediator)
        {
            return await mediator.Send(new GetAllLocationsQuery());
        }

        public async Task<LocationDto?> GetLocation(int id, IMediator mediator)
        {
            return await mediator.Send(new GetLocationByIdQuery(id));
        }

        public async Task<IReadOnlyList<SupplierDto>> GetSuppliers(IMediator mediator)
        {
            return await mediator.Send(new GetAllSuppliersQuery());
        }

        public async Task<SupplierDto?> GetSupplier(string code, IMediator mediator)
        {
            return await mediator.Send(new GetSupplierByCodeQuery(code));
        }

        public async Task<IReadOnlyList<StateDto>> GetStates(IMediator mediator)
        {
            return await mediator.Send(new GetAllStatesQuery());
        }

        public async Task<StateDto?> GetState(string code, IMediator mediator)
        {
            return await mediator.Send(new GetStateByCodeQuery(code));
        }

        public async Task<IReadOnlyList<CityDto>> GetCities(IMediator mediator)
        {
            return await mediator.Send(new GetAllCitiesQuery());
        }

        public async Task<CityDto?> GetCity(string code, IMediator mediator)
        {
            return await mediator.Send(new GetCityByCodeQuery(code));
        }

        public async Task<IReadOnlyList<CityDto>> GetCitiesByState(string stateCode, IMediator mediator)
        {
            return await mediator.Send(new GetCitiesByStateCodeQuery(stateCode));
        }
    }

    public class Mutation
    {
        public async Task<int> CreateCompanyUnit(string code, string name, IMediator mediator)
        {
            return await mediator.Send(new CreateCompanyUnitCommand(code, name));
        }

        public async Task<bool> UpdateCompanyUnit(int id, string code, string name, IMediator mediator)
        {
            return await mediator.Send(new UpdateCompanyUnitCommand(id, code, name));
        }

        public async Task<bool> DeleteCompanyUnit(int id, IMediator mediator)
        {
            return await mediator.Send(new DeleteCompanyUnitCommand(id));
        }

        public async Task<int> CreateLocation(string name, IMediator mediator)
        {
            return await mediator.Send(new CreateLocationCommand(name));
        }

        public async Task<bool> UpdateLocation(int id, string name, IMediator mediator)
        {
            return await mediator.Send(new UpdateLocationCommand(id, name));
        }

        public async Task<bool> DeleteLocation(int id, IMediator mediator)
        {
            return await mediator.Send(new DeleteLocationCommand(id));
        }

        public async Task<string> CreateSupplier(string code, string name, string? details, string entryId, decimal entryNumber, IMediator mediator)
        {
            return await mediator.Send(new CreateSupplierCommand(code, name, details, entryId, entryNumber));
        }

        public async Task<bool> UpdateSupplier(string code, string name, string? details, IMediator mediator)
        {
            return await mediator.Send(new UpdateSupplierCommand(code, name, details));
        }

        public async Task<bool> DeleteSupplier(string code, IMediator mediator)
        {
            return await mediator.Send(new DeleteSupplierCommand(code));
        }

        public async Task<string> CreateState(string code, string name, IMediator mediator)
        {
            return await mediator.Send(new CreateStateCommand(code, name));
        }

        public async Task<string> CreateCity(string code, string name, string stateCode, IMediator mediator)
        {
            return await mediator.Send(new CreateCityCommand(code, name, stateCode));
        }
    }
}
