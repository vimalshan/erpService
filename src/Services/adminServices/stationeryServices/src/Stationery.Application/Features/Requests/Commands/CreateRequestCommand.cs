using MediatR;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;
using Stationery.Domain.Events;
using MassTransit;

namespace Stationery.Application.Features.Requests.Commands;

public record CreateRequestCommand(
    long RequestedBy,
    long LocationId,
    string UnitCode,
    List<RequestDetailDto> Details
) : IRequest<long>;

public record RequestDetailDto(
    long StationaryId,
    long DeptId,
    DateTime ExpectedDate,
    long RequestedQty
);

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateRequestCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<long> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var requestMain = new RequestMain
        {
            RequestedBy = request.RequestedBy,
            RequestedOn = DateTime.UtcNow,
            LocationId = request.LocationId,
            UnitCode = request.UnitCode
        };

        foreach (var detail in request.Details)
        {
            requestMain.Details.Add(new RequestSub
            {
                StationaryId = detail.StationaryId,
                DeptId = detail.DeptId,
                ExpectedDate = detail.ExpectedDate,
                RequestedQty = detail.RequestedQty,
                Status = "P",
                UpdatedBy = request.RequestedBy,
                UpdatedOn = DateTime.UtcNow
            });
        }

        await _unitOfWork.Repository<RequestMain>().AddAsync(requestMain);
        await _unitOfWork.CompleteAsync();

        // Trigger Domain Event / Background processing via MassTransit
        await _publishEndpoint.Publish(new RequestCreatedEvent(requestMain), cancellationToken);

        return requestMain.Id;
    }
}
