using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetUserRightsByUserIdQuery : IRequest<IEnumerable<DTOs.UserRightDto>>
{
    public string UserId { get; set; } = string.Empty;

    public class Handler : IRequestHandler<GetUserRightsByUserIdQuery, IEnumerable<DTOs.UserRightDto>>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.UserRightDto>> Handle(GetUserRightsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userRights = await _unitOfWork.UserRights.GetByUserIdAsync(request.UserId, cancellationToken);
            return userRights.Select(ur => new DTOs.UserRightDto
            {
                Id = ur.Id,
                UserId = ur.UserId,
                PinNumber = ur.PinNumber,
                RightCode = ur.RightCode,
                BusinessCode = ur.BusinessCode,
                UnitCode = ur.UnitCode,
                RightMode = ur.RightMode
            }).ToList();
        }
    }
}
