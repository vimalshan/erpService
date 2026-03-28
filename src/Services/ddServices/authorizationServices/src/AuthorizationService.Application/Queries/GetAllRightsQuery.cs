using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetAllRightsQuery : IRequest<IEnumerable<DTOs.RightDto>>
{
    public class Handler : IRequestHandler<GetAllRightsQuery, IEnumerable<DTOs.RightDto>>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DTOs.RightDto>> Handle(GetAllRightsQuery request, CancellationToken cancellationToken)
        {
            var rights = await _unitOfWork.Rights.GetAllAsync(cancellationToken);
            return rights.Select(r => new DTOs.RightDto
            {
                Id = r.Id,
                RightCode = r.RightCode,
                RightDescription = r.RightDescription,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }
    }
}
