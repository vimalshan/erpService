using MediatR;

namespace AuthorizationService.Application.Commands;

public class CreateTrackerRightCommand : IRequest<long>
{
    public string UserId { get; set; } = string.Empty;
    public decimal? PinNumber { get; set; }
    public string? TrackerMode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public char? TrackerRights { get; set; }
    public char? VtcRights { get; set; }
    public char? RepresentingUnit { get; set; }
    public char? LetRight { get; set; }
    public char? CarRight { get; set; }

    public class Handler : IRequestHandler<CreateTrackerRightCommand, long>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateTrackerRightCommand request, CancellationToken cancellationToken)
        {
            var trackerRight = new AuthorizationService.Domain.Entities.TrackerRight(
                request.UserId,
                request.PinNumber,
                request.BusinessCode)
            {
                TrackerMode = request.TrackerMode,
                UnitCode = request.UnitCode,
                TrackerRights = request.TrackerRights,
                VtcRights = request.VtcRights,
                RepresentingUnit = request.RepresentingUnit,
                LetRight = request.LetRight,
                CarRight = request.CarRight
            };

            await _unitOfWork.TrackerRights.AddAsync(trackerRight, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return trackerRight.Id;
        }
    }
}
