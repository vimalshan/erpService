using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetTrackerRightsByUserIdQuery : IRequest<IEnumerable<DTOs.TrackerRightDto>>
{
    public string UserId { get; set; } = string.Empty;

    public class Handler : IRequestHandler<GetTrackerRightsByUserIdQuery, IEnumerable<DTOs.TrackerRightDto>>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.TrackerRightDto>> Handle(GetTrackerRightsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var trackerRights = await _unitOfWork.TrackerRights.GetByUserIdAsync(request.UserId, cancellationToken);
            return trackerRights.Select(tr => new DTOs.TrackerRightDto
            {
                Id = tr.Id,
                UserId = tr.UserId,
                PinNumber = tr.PinNumber,
                TrackerMode = tr.TrackerMode,
                BusinessCode = tr.BusinessCode,
                UnitCode = tr.UnitCode,
                TrackerRights = tr.TrackerRights,
                VtcRights = tr.VtcRights,
                RepresentingUnit = tr.RepresentingUnit,
                LetRight = tr.LetRight,
                CarRight = tr.CarRight,
                HasTrackerAccess = tr.HasTrackerAccess,
                HasVtcAccess = tr.HasVtcAccess
            }).ToList();
        }
    }
}
