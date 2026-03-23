using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;
using VisitorServices.Domain.Aggregates;

namespace VisitorServices.Application.Visitors.Queries.GetActiveVisitors;

public sealed class GetActiveVisitorsQueryHandler(IVisitorRepository visitorRepository)
    : IRequestHandler<GetActiveVisitorsQuery, IEnumerable<VisitorDto>>
{
    public async Task<IEnumerable<VisitorDto>> Handle(GetActiveVisitorsQuery request, CancellationToken cancellationToken)
    {
        var visitors = await visitorRepository.GetActiveVisitorsAsync(cancellationToken);
        return visitors.Select(MapToDto);
    }

    private static VisitorDto MapToDto(VisitorAggregate v) => new(
        v.Id, v.Name, v.IdDocument.ToChar().ToString(), v.IdDocument.IdNumber,
        v.ContactInfo.PhoneNumber, v.ContactInfo.Email, v.Company, v.Purpose,
        v.CheckInTime, v.CheckOutTime, ((char)(int)v.Status).ToString(),
        v.WhomToVisit, v.EnteredOn, v.EnteredBy);
}
