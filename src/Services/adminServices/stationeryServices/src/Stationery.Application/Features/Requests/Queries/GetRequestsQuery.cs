using AutoMapper;
using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Requests.Queries;

public record GetRequestsQuery(long? LocationId = null, string? Status = null) : IRequest<IEnumerable<RequestSummaryDto>>;

public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, IEnumerable<RequestSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RequestSummaryDto>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RequestMain> requests;

        if (request.LocationId.HasValue)
            requests = await _unitOfWork.Repository<RequestMain>()
                .FindAsync(r => r.LocationId == request.LocationId.Value);
        else
            requests = await _unitOfWork.Repository<RequestMain>().GetAllAsync();

        // Enrich with details counts if status filter is needed
        if (!string.IsNullOrEmpty(request.Status))
        {
            var allSubs = await _unitOfWork.Repository<RequestSub>()
                .FindAsync(s => s.Status == request.Status);
            var requestIdsWithStatus = allSubs.Select(s => s.RequestId).ToHashSet();
            requests = requests.Where(r => requestIdsWithStatus.Contains(r.Id));
        }

        var result = new List<RequestSummaryDto>();
        foreach (var r in requests)
        {
            var subs = await _unitOfWork.Repository<RequestSub>()
                .FindAsync(s => s.RequestId == r.Id);
            var subList = subs.ToList();
            result.Add(new RequestSummaryDto(
                r.Id,
                r.RequestedBy,
                r.RequestedOn,
                r.LocationId,
                r.UnitCode,
                subList.Count,
                subList.Count(s => s.Status == "P"),
                subList.Count(s => s.Status == "A")
            ));
        }

        return result;
    }
}
