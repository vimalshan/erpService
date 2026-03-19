using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using MasterData.Application.DTOs;

#nullable enable

namespace MasterData.Application.Queries.CompanyUnit
{
    /// <summary>
    /// Query to get a Company Unit by ID
    /// </summary>
    public record GetCompanyUnitByIdQuery(int Id) : IRequest<CompanyUnitDto?>;

    /// <summary>
    /// Query to get all Company Units
    /// </summary>
    public record GetAllCompanyUnitsQuery : IRequest<IReadOnlyList<CompanyUnitDto>>;
}

namespace MasterData.Application.Queries.Location
{
    /// <summary>
    /// Query to get a Location by ID
    /// </summary>
    public record GetLocationByIdQuery(int Id) : IRequest<LocationDto?>;

    /// <summary>
    /// Query to get all Locations
    /// </summary>
    public record GetAllLocationsQuery : IRequest<IReadOnlyList<LocationDto>>;
}

namespace MasterData.Application.Queries.Supplier
{
    /// <summary>
    /// Query to get a Supplier by Code
    /// </summary>
    public record GetSupplierByCodeQuery(string Code) : IRequest<SupplierDto?>;

    /// <summary>
    /// Query to get all Suppliers
    /// </summary>
    public record GetAllSuppliersQuery : IRequest<IReadOnlyList<SupplierDto>>;
}

namespace MasterData.Application.Queries.State
{
    /// <summary>
    /// Query to get a State by Code
    /// </summary>
    public record GetStateByCodeQuery(string Code) : IRequest<StateDto?>;

    /// <summary>
    /// Query to get all States
    /// </summary>
    public record GetAllStatesQuery : IRequest<IReadOnlyList<StateDto>>;
}

namespace MasterData.Application.Queries.City
{
    /// <summary>
    /// Query to get a City by Code
    /// </summary>
    public record GetCityByCodeQuery(string Code) : IRequest<CityDto?>;

    /// <summary>
    /// Query to get all Cities
    /// </summary>
    public record GetAllCitiesQuery : IRequest<IReadOnlyList<CityDto>>;

    /// <summary>
    /// Query to get Cities by State Code
    /// </summary>
    public record GetCitiesByStateCodeQuery(string StateCode) : IRequest<IReadOnlyList<CityDto>>;
}
