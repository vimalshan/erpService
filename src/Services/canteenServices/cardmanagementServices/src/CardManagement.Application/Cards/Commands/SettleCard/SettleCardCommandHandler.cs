using AutoMapper;
using MediatR;
using CardManagement.Application.Common.DTOs;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Application.Cards.Commands.SettleCard;

public class SettleCardCommandHandler : IRequestHandler<SettleCardCommand, CardSettlementDto>
{
    private readonly ICardSettlementRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public SettleCardCommandHandler(ICardSettlementRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CardSettlementDto> Handle(SettleCardCommand request, CancellationToken cancellationToken)
    {
        var entity = CardSettlement.Create(request.SysId, request.CanteenUnit, request.CardNumber, request.SettlementDate, _currentUser.UserId);
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CardSettlementDto>(entity);
    }
}
