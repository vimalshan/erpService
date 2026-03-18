using MediatR;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.Cards.Commands.CloseGuestCard;

public class CloseGuestCardCommandHandler : IRequestHandler<CloseGuestCardCommand, bool>
{
    private readonly IGuestCardMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CloseGuestCardCommandHandler(IGuestCardMasterRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(CloseGuestCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CanteenUnit, cancellationToken)
            ?? throw new KeyNotFoundException($"Guest card {request.CanteenUnit} not found.");

        entity.Close(_currentUser.UserId);
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
