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
    [QueryType]
    public class Query
    {
        private readonly IMediator _mediator;

        public Query(IMediator mediator)
        {
            _mediator = mediator;
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<CompanyUnitDto>>>>))]
        public async Task<IReadOnlyList<CompanyUnitDto>> GetCompanyUnits()
        {
            return await _mediator.Send(new GetAllCompanyUnitsQuery());
        }

        [GraphQLType(typeof(ObjectType<CompanyUnitDto>))]
        public async Task<CompanyUnitDto?> GetCompanyUnit(int id)
        {
            return await _mediator.Send(new GetCompanyUnitByIdQuery(id));
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<LocationDto>>>>))]
        public async Task<IReadOnlyList<LocationDto>> GetLocations()
        {
            return await _mediator.Send(new GetAllLocationsQuery());
        }

        [GraphQLType(typeof(ObjectType<LocationDto>))]
        public async Task<LocationDto?> GetLocation(int id)
        {
            return await _mediator.Send(new GetLocationByIdQuery(id));
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<SupplierDto>>>>))]
        public async Task<IReadOnlyList<SupplierDto>> GetSuppliers()
        {
            return await _mediator.Send(new GetAllSuppliersQuery());
        }

        [GraphQLType(typeof(ObjectType<SupplierDto>))]
        public async Task<SupplierDto?> GetSupplier(string code)
        {
            return await _mediator.Send(new GetSupplierByCodeQuery(code));
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<StateDto>>>>))]
        public async Task<IReadOnlyList<StateDto>> GetStates()
        {
            return await _mediator.Send(new GetAllStatesQuery());
        }

        [GraphQLType(typeof(ObjectType<StateDto>))]
        public async Task<StateDto?> GetState(string code)
        {
            return await _mediator.Send(new GetStateByCodeQuery(code));
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<CityDto>>>>))]
        public async Task<IReadOnlyList<CityDto>> GetCities()
        {
            return await _mediator.Send(new GetAllCitiesQuery());
        }

        [GraphQLType(typeof(ObjectType<CityDto>))]
        public async Task<CityDto?> GetCity(string code)
        {
            return await _mediator.Send(new GetCityByCodeQuery(code));
        }

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ObjectType<CityDto>>>>))]
        public async Task<IReadOnlyList<CityDto>> GetCitiesByState(string stateCode)
        {
            return await _mediator.Send(new GetCitiesByStateCodeQuery(stateCode));
        }
    }

    [MutationType]
    public class Mutation
    {
        private readonly IMediator _mediator;

        public Mutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> CreateCompanyUnit(string code, string name)
        {
            return await _mediator.Send(new CreateCompanyUnitCommand(code, name));
        }

        public async Task<bool> UpdateCompanyUnit(int id, string code, string name)
        {
            return await _mediator.Send(new UpdateCompanyUnitCommand(id, code, name));
        }

        public async Task<bool> DeleteCompanyUnit(int id)
        {
            return await _mediator.Send(new DeleteCompanyUnitCommand(id));
        }

        public async Task<int> CreateLocation(string name)
        {
            return await _mediator.Send(new CreateLocationCommand(name));
        }

        public async Task<bool> UpdateLocation(int id, string name)
        {
            return await _mediator.Send(new UpdateLocationCommand(id, name));
        }

        public async Task<bool> DeleteLocation(int id)
        {
            return await _mediator.Send(new DeleteLocationCommand(id));
        }

        public async Task<string> CreateSupplier(string code, string name, string? details, string entryId, decimal entryNumber)
        {
            return await _mediator.Send(new CreateSupplierCommand(code, name, details, entryId, entryNumber));
        }

        public async Task<bool> UpdateSupplier(string code, string name, string? details)
        {
            return await _mediator.Send(new UpdateSupplierCommand(code, name, details));
        }

        public async Task<bool> DeleteSupplier(string code)
        {
            return await _mediator.Send(new DeleteSupplierCommand(code));
        }

        public async Task<string> CreateState(string code, string name)
        {
            return await _mediator.Send(new CreateStateCommand(code, name));
        }

        public async Task<string> CreateCity(string code, string name, string stateCode)
        {
            return await _mediator.Send(new CreateCityCommand(code, name, stateCode));
        }
    }
}
