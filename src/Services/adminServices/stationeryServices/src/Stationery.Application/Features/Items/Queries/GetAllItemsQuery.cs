using AutoMapper;
using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Items.Queries;

public record GetAllItemsQuery(long? LocationId = null) : IRequest<IEnumerable<ItemDto>>;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<StationaryMaster> items;

        if (request.LocationId.HasValue)
            items = await _unitOfWork.Repository<StationaryMaster>()
                .FindAsync(i => i.LocId == request.LocationId.Value && i.Closed == "N");
        else
            items = await _unitOfWork.Repository<StationaryMaster>()
                .FindAsync(i => i.Closed == "N");

        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }
}
