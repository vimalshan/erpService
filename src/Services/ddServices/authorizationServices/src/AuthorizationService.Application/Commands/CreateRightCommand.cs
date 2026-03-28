using MediatR;

namespace AuthorizationService.Application.Commands;

public class CreateRightCommand : IRequest<long>
{
    public decimal RightCode { get; set; }
    public string? RightDescription { get; set; }

    public class Handler : IRequestHandler<CreateRightCommand, long>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateRightCommand request, CancellationToken cancellationToken)
        {
            var right = new AuthorizationService.Domain.Entities.Right(
                request.RightCode,
                request.RightDescription);

            await _unitOfWork.Rights.AddAsync(right, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return right.Id;
        }
    }
}
