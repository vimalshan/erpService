using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Queries.Coupon;

public record GetAllCouponsQuery : IRequest<IReadOnlyList<CouponDto>>;
public record GetCouponsByAirlineQuery(string Airline) : IRequest<IReadOnlyList<CouponDto>>;
public record GetExpiredCouponsQuery : IRequest<IReadOnlyList<CouponDto>>;
