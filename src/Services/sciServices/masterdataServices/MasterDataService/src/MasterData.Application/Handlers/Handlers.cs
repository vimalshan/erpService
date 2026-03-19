using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using AutoMapper;
using MasterData.Application.Commands.CompanyUnit;
using MasterData.Application.Queries.CompanyUnit;
using MasterData.Application.DTOs;
using MasterData.Domain.Aggregates;
using MasterData.Domain.Entities;

#nullable enable

namespace MasterData.Application.Handlers.CompanyUnit
{
    /// <summary>
    /// Handler for CreateCompanyUnitCommand
    /// </summary>
    public class CreateCompanyUnitHandler : IRequestHandler<CreateCompanyUnitCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyUnitHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCompanyUnitCommand request, CancellationToken cancellationToken)
        {
            var companyUnit = CompanyUnitAggregate.Create(request.Code, request.Name);
            await _unitOfWork.CompanyUnits.AddAsync(companyUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return companyUnit.Id;
        }
    }

    /// <summary>
    /// Handler for UpdateCompanyUnitCommand
    /// </summary>
    public class UpdateCompanyUnitHandler : IRequestHandler<UpdateCompanyUnitCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyUnitHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCompanyUnitCommand request, CancellationToken cancellationToken)
        {
            var companyUnit = await _unitOfWork.CompanyUnits.GetByIdAsync(request.Id);
            if (companyUnit == null)
                return false;

            companyUnit.Update(request.Code, request.Name);
            await _unitOfWork.CompanyUnits.UpdateAsync(companyUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    /// <summary>
    /// Handler for DeleteCompanyUnitCommand
    /// </summary>
    public class DeleteCompanyUnitHandler : IRequestHandler<DeleteCompanyUnitCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyUnitHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCompanyUnitCommand request, CancellationToken cancellationToken)
        {
            var companyUnit = await _unitOfWork.CompanyUnits.GetByIdAsync(request.Id);
            if (companyUnit == null)
                return false;

            companyUnit.Delete();
            await _unitOfWork.CompanyUnits.UpdateAsync(companyUnit);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    /// <summary>
    /// Handler for GetCompanyUnitByIdQuery
    /// </summary>
    public class GetCompanyUnitByIdHandler : IRequestHandler<GetCompanyUnitByIdQuery, CompanyUnitDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyUnitByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyUnitDto?> Handle(GetCompanyUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var companyUnit = await _unitOfWork.CompanyUnits.GetByIdAsync(request.Id);
            return companyUnit == null ? null : _mapper.Map<CompanyUnitDto>(companyUnit);
        }
    }

    /// <summary>
    /// Handler for GetAllCompanyUnitsQuery
    /// </summary>
    public class GetAllCompanyUnitsHandler : IRequestHandler<GetAllCompanyUnitsQuery, IReadOnlyList<CompanyUnitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCompanyUnitsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CompanyUnitDto>> Handle(GetAllCompanyUnitsQuery request, CancellationToken cancellationToken)
        {
            var companyUnits = await _unitOfWork.CompanyUnits.GetAllAsync();
            return _mapper.Map<List<CompanyUnitDto>>(companyUnits.Where(x => !x.IsDeleted).ToList()).AsReadOnly();
        }
    }
}

namespace MasterData.Application.Handlers.Location
{
    using MasterData.Application.Commands.Location;
    using MasterData.Application.Queries.Location;
    using MasterData.Domain.Entities;

    /// <summary>
    /// Handler for CreateLocationCommand
    /// </summary>
    public class CreateLocationHandler : IRequestHandler<CreateLocationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLocationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = LocationAggregate.Create(request.Name);
            await _unitOfWork.Locations.AddAsync(location);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return location.Id;
        }
    }

    /// <summary>
    /// Handler for UpdateLocationCommand
    /// </summary>
    public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLocationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(request.Id);
            if (location == null)
                return false;

            location.Update(request.Name);
            await _unitOfWork.Locations.UpdateAsync(location);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    /// <summary>
    /// Handler for DeleteLocationCommand
    /// </summary>
    public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLocationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(request.Id);
            if (location == null)
                return false;

            await _unitOfWork.Locations.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    /// <summary>
    /// Handler for GetLocationByIdQuery
    /// </summary>
    public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, LocationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLocationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LocationDto?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(request.Id);
            return location == null ? null : _mapper.Map<LocationDto>(location);
        }
    }

    /// <summary>
    /// Handler for GetAllLocationsQuery
    /// </summary>
    public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, IReadOnlyList<LocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLocationsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _unitOfWork.Locations.GetAllAsync();
            return _mapper.Map<List<LocationDto>>(locations.Where(x => !x.IsDeleted).ToList()).AsReadOnly();
        }
    }
}

namespace MasterData.Application.Handlers.Supplier
{
    using MasterData.Application.Commands.Supplier;
    using MasterData.Application.Queries.Supplier;
    using MasterData.Domain.Entities;

    /// <summary>
    /// Handler for CreateSupplierCommand
    /// </summary>
    public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplierHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = SupplierAggregate.Create(request.Code, request.Name, request.Details, request.EntryId, request.EntryNumber);
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return supplier.Code;
        }
    }

    /// <summary>
    /// Handler for UpdateSupplierCommand
    /// </summary>
    public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupplierHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _unitOfWork.Suppliers.GetByCodeAsync(request.Code);
            if (supplier == null)
                return false;

            supplier.Update(request.Name, request.Details);
            await _unitOfWork.Suppliers.UpdateAsync(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    /// <summary>
    /// Handler for GetSupplierByCodeQuery
    /// </summary>
    public class GetSupplierByCodeHandler : IRequestHandler<GetSupplierByCodeQuery, SupplierDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSupplierByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupplierDto?> Handle(GetSupplierByCodeQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _unitOfWork.Suppliers.GetByCodeAsync(request.Code);
            return supplier == null ? null : _mapper.Map<SupplierDto>(supplier);
        }
    }

    /// <summary>
    /// Handler for GetAllSuppliersQuery
    /// </summary>
    public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IReadOnlyList<SupplierDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSuppliersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
            return _mapper.Map<List<SupplierDto>>(suppliers).AsReadOnly();
        }
    }
}

namespace MasterData.Application.Handlers.State
{
    using MasterData.Application.Commands.State;
    using MasterData.Application.Queries.State;
    using MasterData.Domain.Entities;

    /// <summary>
    /// Handler for CreateStateCommand
    /// </summary>
    public class CreateStateHandler : IRequestHandler<CreateStateCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateStateCommand request, CancellationToken cancellationToken)
        {
            var state = StateAggregate.Create(request.Code, request.Name);
            await _unitOfWork.States.AddAsync(state);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return state.Code;
        }
    }

    /// <summary>
    /// Handler for GetStateByCodeQuery
    /// </summary>
    public class GetStateByCodeHandler : IRequestHandler<GetStateByCodeQuery, StateDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStateByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StateDto?> Handle(GetStateByCodeQuery request, CancellationToken cancellationToken)
        {
            var state = await _unitOfWork.States.GetByCodeAsync(request.Code);
            return state == null ? null : _mapper.Map<StateDto>(state);
        }
    }

    /// <summary>
    /// Handler for GetAllStatesQuery
    /// </summary>
    public class GetAllStatesHandler : IRequestHandler<GetAllStatesQuery, IReadOnlyList<StateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllStatesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<StateDto>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            var states = await _unitOfWork.States.GetAllAsync();
            return _mapper.Map<List<StateDto>>(states).AsReadOnly();
        }
    }
}

namespace MasterData.Application.Handlers.City
{
    using MasterData.Application.Commands.City;
    using MasterData.Application.Queries.City;
    using MasterData.Domain.Entities;

    /// <summary>
    /// Handler for CreateCityCommand
    /// </summary>
    public class CreateCityHandler : IRequestHandler<CreateCityCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = CityAggregate.Create(request.Code, request.Name, request.StateCode);
            await _unitOfWork.Cities.AddAsync(city);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return city.Code;
        }
    }

    /// <summary>
    /// Handler for GetCityByCodeQuery
    /// </summary>
    public class GetCityByCodeHandler : IRequestHandler<GetCityByCodeQuery, CityDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCityByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CityDto?> Handle(GetCityByCodeQuery request, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.Cities.GetByCodeAsync(request.Code);
            return city == null ? null : _mapper.Map<CityDto>(city);
        }
    }

    /// <summary>
    /// Handler for GetAllCitiesQuery
    /// </summary>
    public class GetAllCitiesHandler : IRequestHandler<GetAllCitiesQuery, IReadOnlyList<CityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCitiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _unitOfWork.Cities.GetAllAsync();
            return _mapper.Map<List<CityDto>>(cities).AsReadOnly();
        }
    }

    /// <summary>
    /// Handler for GetCitiesByStateCodeQuery
    /// </summary>
    public class GetCitiesByStateCodeHandler : IRequestHandler<GetCitiesByStateCodeQuery, IReadOnlyList<CityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCitiesByStateCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CityDto>> Handle(GetCitiesByStateCodeQuery request, CancellationToken cancellationToken)
        {
            var cities = await _unitOfWork.Cities.GetByStateCodeAsync(request.StateCode);
            return _mapper.Map<List<CityDto>>(cities).AsReadOnly();
        }
    }
}
