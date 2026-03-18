using MediatR;
using LocationService.Application.Commands.Locations;
using LocationService.Application.Queries.Locations;
using LocationService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LocationService.API.Controllers
{
    /// <summary>
    /// Location API endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(IMediator mediator, ILogger<LocationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<LocationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(locations);
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var location = await _mediator.Send(new GetLocationByIdQuery(id));
            if (location == null)
                return NotFound();
            return Ok(location);
        }

        /// <summary>
        /// Get location by code
        /// </summary>
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            var location = await _mediator.Send(new GetLocationByCodeQuery(code));
            if (location == null)
                return NotFound();
            return Ok(location);
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateLocationDto dto)
        {
            var command = new CreateLocationCommand
            {
                LocationCode = dto.LocationCode,
                LocationName = dto.LocationName,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                UserId = 1 // TODO: Get from current user
            };

            var location = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = location.LocationId }, location);
        }

        /// <summary>
        /// Update a location
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateLocationDto dto)
        {
            var command = new UpdateLocationCommand
            {
                LocationId = id,
                LocationName = dto.LocationName,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                UserId = 1 // TODO: Get from current user
            };

            var location = await _mediator.Send(command);
            return Ok(location);
        }

        /// <summary>
        /// Delete a location
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteLocationCommand { LocationId = id, UserId = 1 };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Get active locations
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IReadOnlyList<LocationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            var locations = await _mediator.Send(new GetActiveLocationsQuery());
            return Ok(locations);
        }

        /// <summary>
        /// Search locations by name
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<LocationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            var locations = await _mediator.Send(new SearchLocationsByNameQuery(searchText));
            return Ok(locations);
        }
    }
}
