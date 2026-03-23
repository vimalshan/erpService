using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;
using VisitorServices.Domain.Aggregates;

namespace VisitorServices.Application.Visitors.Commands.RegisterVisitor;

public sealed class RegisterVisitorCommandHandler(
    IVisitorRepository visitorRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterVisitorCommand, VisitorDto>
{
    public async Task<VisitorDto> Handle(RegisterVisitorCommand request, CancellationToken cancellationToken)
    {
        var id = await visitorRepository.GetNextIdAsync(cancellationToken);

        var visitor = VisitorAggregate.Register(
            id,
            request.VisitorName,
            request.IdType,
            request.IdNumber,
            request.PhoneNumber,
            request.Email,
            request.Company,
            request.Purpose,
            request.WhomToVisit,
            request.EnteredBy);

        await visitorRepository.AddAsync(visitor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(visitor);
    }

    private static VisitorDto MapToDto(VisitorAggregate v) => new(
        v.Id,
        v.Name,
        v.IdDocument.ToChar().ToString(),
        v.IdDocument.IdNumber,
        v.ContactInfo.PhoneNumber,
        v.ContactInfo.Email,
        v.Company,
        v.Purpose,
        v.CheckInTime,
        v.CheckOutTime,
        ((char)(int)v.Status).ToString(),
        v.WhomToVisit,
        v.EnteredOn,
        v.EnteredBy);
}
