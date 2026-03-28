using MediatR;

namespace AuthorizationService.Application.Commands;

public class CreateUserRightCommand : IRequest<long>
{
    public string? UserId { get; set; }
    public decimal? PinNumber { get; set; }
    public decimal? RightCode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public decimal? RightMode { get; set; }

    public class Handler : IRequestHandler<CreateUserRightCommand, long>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateUserRightCommand request, CancellationToken cancellationToken)
        {
            var userRight = new AuthorizationService.Domain.Entities.UserRight(
                request.UserId,
                request.PinNumber,
                request.RightCode)
            {
                BusinessCode = request.BusinessCode,
                UnitCode = request.UnitCode,
                RightMode = request.RightMode
            };

            await _unitOfWork.UserRights.AddAsync(userRight, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return userRight.Id;
        }
    }
}
