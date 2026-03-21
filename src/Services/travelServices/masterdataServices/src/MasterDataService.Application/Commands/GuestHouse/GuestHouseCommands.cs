using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Commands.GuestHouse;

public record CreateGuestHouseCommand(long AdminCode, string GuestHouseName, long DailyAmount) : IRequest<GuestHouseDto>;
public record UpdateGuestHouseCommand(long Id, string GuestHouseName, long DailyAmount) : IRequest<GuestHouseDto>;
public record DeleteGuestHouseCommand(long Id) : IRequest<bool>;
