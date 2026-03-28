using MediatR;

namespace AuthorizationService.Application.Commands;

public class DeleteUserRightCommand : IRequest<bool>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteUserRightCommand, bool>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserRightCommand request, CancellationToken cancellationToken)
        {
            var userRight = await _unitOfWork.UserRights.GetByIdAsync(request.Id, cancellationToken);
            if (userRight == null)
                return false;

            await _unitOfWork.UserRights.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
