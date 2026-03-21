using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Queries.Coupon;

public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQuery, IReadOnlyList<CouponDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CouponDto>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Coupons.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CouponDto>>(entities);
    }
}

public class GetCouponsByAirlineQueryHandler : IRequestHandler<GetCouponsByAirlineQuery, IReadOnlyList<CouponDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCouponsByAirlineQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CouponDto>> Handle(GetCouponsByAirlineQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Coupons.GetByAirlineAsync(request.Airline, cancellationToken);
        return _mapper.Map<IReadOnlyList<CouponDto>>(entities);
    }
}

public class GetExpiredCouponsQueryHandler : IRequestHandler<GetExpiredCouponsQuery, IReadOnlyList<CouponDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetExpiredCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CouponDto>> Handle(GetExpiredCouponsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Coupons.GetExpiredCouponsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CouponDto>>(entities);
    }
}
