using AutoMapper;
using MediatR;
using CardManagement.Application.Common.DTOs;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.Cards.Commands.CreateGuestCard;

public class CreateGuestCardCommandHandler : IRequestHandler<CreateGuestCardCommand, GuestCardDto>
{
    private readonly IGuestCardMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IMessagePublisher _publisher;

    public CreateGuestCardCommandHandler(
        IGuestCardMasterRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser,
        IMessagePublisher publisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public async Task<GuestCardDto> Handle(CreateGuestCardCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByCardNumberAsync(request.CardNumber, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"Card number '{request.CardNumber}' already exists.");

        var entity = GuestCardMaster.Create(
            request.CanteenUnit, request.CardSequence, request.CardNumber, request.CardName,
            request.CardType, request.ReportingUnit, request.ReportingDepartment,
            request.EffectiveDate, _currentUser.UserId);

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.PublishAsync(new { entity.CanteenUnit, entity.CardNumber, entity.CardName }, ct: cancellationToken);

        return _mapper.Map<GuestCardDto>(entity);
    }
}
