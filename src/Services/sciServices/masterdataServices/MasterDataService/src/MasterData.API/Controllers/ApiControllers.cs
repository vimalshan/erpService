using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace MasterData.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CompanyUnitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyUnitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all company units
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<CompanyUnitDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCompanyUnitsQuery());
            return Ok(ApiResponse<IReadOnlyList<CompanyUnitDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Get a company unit by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CompanyUnitDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetCompanyUnitByIdQuery(id));
            if (result == null)
                return NotFound(ApiResponse<CompanyUnitDto>.ErrorResponse("Company Unit not found"));

            return Ok(ApiResponse<CompanyUnitDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Create a new company unit
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateCompanyUnitCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, 
                ApiResponse<int>.SuccessResponse(result, "Company Unit created successfully"));
        }

        /// <summary>
        /// Update a company unit
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdateCompanyUnitCommand command)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<bool>.ErrorResponse("ID mismatch"));

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Company Unit not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Company Unit updated successfully"));
        }

        /// <summary>
        /// Delete a company unit
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCompanyUnitCommand(id));
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Company Unit not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Company Unit deleted successfully"));
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<LocationDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(ApiResponse<IReadOnlyList<LocationDto>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LocationDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetLocationByIdQuery(id));
            if (result == null)
                return NotFound(ApiResponse<LocationDto>.ErrorResponse("Location not found"));

            return Ok(ApiResponse<LocationDto>.SuccessResponse(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, 
                ApiResponse<int>.SuccessResponse(result, "Location created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdateLocationCommand command)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<bool>.ErrorResponse("ID mismatch"));

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Location not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Location updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLocationCommand(id));
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Location not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Location deleted successfully"));
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<SupplierDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllSuppliersQuery());
            return Ok(ApiResponse<IReadOnlyList<SupplierDto>>.SuccessResponse(result));
        }

        [HttpGet("{code}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> GetByCode(string code)
        {
            var result = await _mediator.Send(new GetSupplierByCodeQuery(code));
            if (result == null)
                return NotFound(ApiResponse<SupplierDto>.ErrorResponse("Supplier not found"));

            return Ok(ApiResponse<SupplierDto>.SuccessResponse(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> Create(CreateSupplierCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = result }, 
                ApiResponse<string>.SuccessResponse(result, "Supplier created successfully"));
        }

        [HttpPut("{code}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(string code, UpdateSupplierCommand command)
        {
            if (code != command.Code)
                return BadRequest(ApiResponse<bool>.ErrorResponse("Code mismatch"));

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Supplier not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Supplier updated successfully"));
        }

        [HttpDelete("{code}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(string code)
        {
            var result = await _mediator.Send(new DeleteSupplierCommand(code));
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Supplier not found"));

            return Ok(ApiResponse<bool>.SuccessResponse(result, "Supplier deleted successfully"));
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<StateDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllStatesQuery());
            return Ok(ApiResponse<IReadOnlyList<StateDto>>.SuccessResponse(result));
        }

        [HttpGet("{code}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<StateDto>>> GetByCode(string code)
        {
            var result = await _mediator.Send(new GetStateByCodeQuery(code));
            if (result == null)
                return NotFound(ApiResponse<StateDto>.ErrorResponse("State not found"));

            return Ok(ApiResponse<StateDto>.SuccessResponse(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> Create(CreateStateCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = result }, 
                ApiResponse<string>.SuccessResponse(result, "State created successfully"));
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<CityDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCitiesQuery());
            return Ok(ApiResponse<IReadOnlyList<CityDto>>.SuccessResponse(result));
        }

        [HttpGet("{code}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CityDto>>> GetByCode(string code)
        {
            var result = await _mediator.Send(new GetCityByCodeQuery(code));
            if (result == null)
                return NotFound(ApiResponse<CityDto>.ErrorResponse("City not found"));

            return Ok(ApiResponse<CityDto>.SuccessResponse(result));
        }

        [HttpGet("state/{stateCode}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<CityDto>>>> GetByStateCode(string stateCode)
        {
            var result = await _mediator.Send(new GetCitiesByStateCodeQuery(stateCode));
            return Ok(ApiResponse<IReadOnlyList<CityDto>>.SuccessResponse(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> Create(CreateCityCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = result }, 
                ApiResponse<string>.SuccessResponse(result, "City created successfully"));
        }
    }
}
