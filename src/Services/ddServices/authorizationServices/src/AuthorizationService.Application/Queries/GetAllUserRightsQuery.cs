using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetAllUserRightsQuery : IRequest<IEnumerable<DTOs.UserRightDto>>
{
    public class Handler : IRequestHandler<GetAllUserRightsQuery, IEnumerable<DTOs.UserRightDto>>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.UserRightDto>> Handle(GetAllUserRightsQuery request, CancellationToken cancellationToken)
        {
            var userRights = await _unitOfWork.UserRights.GetAllAsync(cancellationToken);
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
