using AutoMapper;
using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Items.Queries;

public record GetItemByIdQuery(long ItemId) : IRequest<ItemDto?>;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetItemByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository<StationaryMaster>().GetByIdAsync(request.ItemId);
        return item == null ? null : _mapper.Map<ItemDto>(item);
    }
}
