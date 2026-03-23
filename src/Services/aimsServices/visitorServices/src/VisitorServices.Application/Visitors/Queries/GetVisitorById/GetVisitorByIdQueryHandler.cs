using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;
using VisitorServices.Domain.Aggregates;

namespace VisitorServices.Application.Visitors.Queries.GetVisitorById;

public sealed class GetVisitorByIdQueryHandler(IVisitorRepository visitorRepository)
    : IRequestHandler<GetVisitorByIdQuery, VisitorDto?>
{
    public async Task<VisitorDto?> Handle(GetVisitorByIdQuery request, CancellationToken cancellationToken)
    {
        var visitor = await visitorRepository.GetByIdAsync(request.VisitorId, cancellationToken);
        return visitor is null ? null : MapToDto(visitor);
    }

    private static VisitorDto MapToDto(VisitorAggregate v) => new(
        v.Id, v.Name, v.IdDocument.ToChar().ToString(), v.IdDocument.IdNumber,
        v.ContactInfo.PhoneNumber, v.ContactInfo.Email, v.Company, v.Purpose,
        v.CheckInTime, v.CheckOutTime, ((char)(int)v.Status).ToString(),
        v.WhomToVisit, v.EnteredOn, v.EnteredBy);
}
