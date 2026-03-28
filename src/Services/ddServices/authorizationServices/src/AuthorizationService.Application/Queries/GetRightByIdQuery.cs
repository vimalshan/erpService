using MediatR;

namespace AuthorizationService.Application.Queries;

public class GetRightByIdQuery : IRequest<DTOs.RightDto?>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<GetRightByIdQuery, DTOs.RightDto?>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DTOs.RightDto?> Handle(GetRightByIdQuery request, CancellationToken cancellationToken)
        {
            var right = await _unitOfWork.Rights.GetByIdAsync(request.Id, cancellationToken);
            if (right == null)
                return null;

            return new DTOs.RightDto
            {
                Id = right.Id,
                RightCode = right.RightCode,
                RightDescription = right.RightDescription,
                CreatedAt = right.CreatedAt,
                UpdatedAt = right.UpdatedAt
            };
        }
    }
}
