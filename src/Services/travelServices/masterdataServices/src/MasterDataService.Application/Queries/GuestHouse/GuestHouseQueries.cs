using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Queries.GuestHouse;

public record GetAllGuestHousesQuery : IRequest<IReadOnlyList<GuestHouseDto>>;
public record GetGuestHouseByIdQuery(long Id) : IRequest<GuestHouseDto?>;
public record GetGuestHouseByAdminCodeQuery(long AdminCode) : IRequest<GuestHouseDto?>;
public record GetGuestHousesWithRoomsQuery : IRequest<IReadOnlyList<GuestHouseDto>>;
