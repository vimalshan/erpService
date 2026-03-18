using AutoMapper;
using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Requests.Queries;

public record GetRequestByIdQuery(long RequestId) : IRequest<RequestDto?>;

public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, RequestDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRequestByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RequestDto?> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var requestMain = await _unitOfWork.Repository<RequestMain>().GetByIdAsync(request.RequestId);
        if (requestMain == null) return null;

        var details = await _unitOfWork.Repository<RequestSub>()
            .FindAsync(s => s.RequestId == request.RequestId);
        requestMain.Details = details.ToList();

        return _mapper.Map<RequestDto>(requestMain);
    }
}
