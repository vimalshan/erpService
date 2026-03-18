using MediatR;
using LocationService.Application.Commands.Rooms;
using LocationService.Application.Queries.Rooms;
using LocationService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LocationService.API.Controllers
{
    /// <summary>
    /// Room API endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(IMediator mediator, ILogger<RoomsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get room by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var room = await _mediator.Send(new GetRoomByIdQuery(id));
            if (room == null)
                return NotFound();
            return Ok(room);
        }

        /// <summary>
        /// Get rooms by location
        /// </summary>
        [HttpGet("location/{locationId}")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLocation(long locationId)
        {
            var rooms = await _mediator.Send(new GetRoomsByLocationQuery(locationId));
            return Ok(rooms);
        }

        /// <summary>
        /// Create a new room
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            var command = new CreateRoomCommand
            {
                LocationId = dto.LocationId,
                RoomCode = dto.RoomCode,
                RoomName = dto.RoomName,
                RoomCapacity = dto.RoomCapacity,
                RoomType = dto.RoomType,
                FloorNumber = dto.FloorNumber,
                UserId = 1 // TODO: Get from current user
            };

            var room = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = room.RoomId }, room);
        }

        /// <summary>
        /// Update a room
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateRoomDto dto)
        {
            var command = new UpdateRoomCommand
            {
                RoomId = id,
                RoomName = dto.RoomName,
                RoomCapacity = dto.RoomCapacity,
                RoomType = dto.RoomType,
                FloorNumber = dto.FloorNumber,
                UserId = 1 // TODO: Get from current user
            };

            var room = await _mediator.Send(command);
            return Ok(room);
        }

        /// <summary>
        /// Delete a room
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteRoomCommand { RoomId = id, UserId = 1 };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Get rooms by type
        /// </summary>
        [HttpGet("type/{roomType}")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByType(string roomType)
        {
            var rooms = await _mediator.Send(new GetRoomsByTypeQuery(roomType));
            return Ok(rooms);
        }

        /// <summary>
        /// Get rooms by capacity
        /// </summary>
        [HttpGet("capacity")]
        [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCapacity([FromQuery] long locationId, [FromQuery] int minCapacity)
        {
            var rooms = await _mediator.Send(new GetRoomsByCapacityQuery(locationId, minCapacity));
            return Ok(rooms);
        }
    }
}
