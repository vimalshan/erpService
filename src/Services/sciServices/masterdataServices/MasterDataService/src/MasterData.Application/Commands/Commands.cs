using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;

#nullable enable

namespace MasterData.Application.Commands.CompanyUnit
{
    /// <summary>
    /// Command to create a new Company Unit
    /// </summary>
    public record CreateCompanyUnitCommand(string Code, string Name) : IRequest<int>;

    /// <summary>
    /// Command to update an existing Company Unit
    /// </summary>
    public record UpdateCompanyUnitCommand(int Id, string Code, string Name) : IRequest<bool>;

    /// <summary>
    /// Command to delete a Company Unit
    /// </summary>
    public record DeleteCompanyUnitCommand(int Id) : IRequest<bool>;
}

namespace MasterData.Application.Commands.Location
{
    /// <summary>
    /// Command to create a new Location
    /// </summary>
    public record CreateLocationCommand(string Name) : IRequest<int>;

    /// <summary>
    /// Command to update an existing Location
    /// </summary>
    public record UpdateLocationCommand(int Id, string Name) : IRequest<bool>;

    /// <summary>
    /// Command to delete a Location
    /// </summary>
    public record DeleteLocationCommand(int Id) : IRequest<bool>;
}

namespace MasterData.Application.Commands.Supplier
{
    /// <summary>
    /// Command to create a new Supplier
    /// </summary>
    public record CreateSupplierCommand(string Code, string Name, string? Details, string EntryId, decimal EntryNumber) : IRequest<string>;

    /// <summary>
    /// Command to update an existing Supplier
    /// </summary>
    public record UpdateSupplierCommand(string Code, string Name, string? Details) : IRequest<bool>;

    /// <summary>
    /// Command to delete a Supplier
    /// </summary>
    public record DeleteSupplierCommand(string Code) : IRequest<bool>;
}

namespace MasterData.Application.Commands.State
{
    /// <summary>
    /// Command to create a new State
    /// </summary>
    public record CreateStateCommand(string Code, string Name) : IRequest<string>;

    /// <summary>
    /// Command to update an existing State
    /// </summary>
    public record UpdateStateCommand(string Code, string Name) : IRequest<bool>;

    /// <summary>
    /// Command to delete a State
    /// </summary>
    public record DeleteStateCommand(string Code) : IRequest<bool>;
}

namespace MasterData.Application.Commands.City
{
    /// <summary>
    /// Command to create a new City
    /// </summary>
    public record CreateCityCommand(string Code, string Name, string StateCode) : IRequest<string>;

    /// <summary>
    /// Command to update an existing City
    /// </summary>
    public record UpdateCityCommand(string Code, string Name, string StateCode) : IRequest<bool>;

    /// <summary>
    /// Command to delete a City
    /// </summary>
    public record DeleteCityCommand(string Code) : IRequest<bool>;
}
