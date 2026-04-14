using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Exceptions;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Commands.CloseReceiving;

public class CloseReceivingCommandHandler : IRequestHandler<CloseReceivingCommand, ReceivingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CloseReceivingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReceivingDto> Handle(CloseReceivingCommand request, CancellationToken cancellationToken)
    {
        var receiving = await _unitOfWork.Receivings.GetByIdAsync(request.ReceivingId, cancellationToken)
            ?? throw new TransactionNotFoundException("Receiving", request.ReceivingId);

        receiving.Close();

        await _unitOfWork.Receivings.UpdateAsync(receiving, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
