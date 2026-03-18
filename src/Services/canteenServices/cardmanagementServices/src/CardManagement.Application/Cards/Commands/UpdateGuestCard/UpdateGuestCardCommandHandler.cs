using MediatR;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.Cards.Commands.UpdateGuestCard;

public class UpdateGuestCardCommandHandler : IRequestHandler<UpdateGuestCardCommand, bool>
{
    private readonly IGuestCardMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateGuestCardCommandHandler(IGuestCardMasterRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(UpdateGuestCardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CanteenUnit, cancellationToken)
            ?? throw new KeyNotFoundException($"Guest card with canteen unit {request.CanteenUnit} not found.");

        entity.Update(request.CardName, request.CardType, request.ReportingUnit, request.ReportingDepartment);
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
