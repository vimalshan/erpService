using MasterDataService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Functions;

public class RoomAvailabilitySyncFunction
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoomAvailabilitySyncFunction> _logger;

    public RoomAvailabilitySyncFunction(IUnitOfWork unitOfWork, ILogger<RoomAvailabilitySyncFunction> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [Function("RoomAvailabilitySync")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Room availability sync started at: {Time}", DateTime.UtcNow);

        var availableRooms = await _unitOfWork.GuestRoomAvailabilities.GetAvailableRoomsAsync();
        _logger.LogInformation("Current available rooms: {Count}", availableRooms.Count);
    }
}
