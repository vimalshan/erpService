using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetReceiving;

public record GetReceivingQuery(int ReceivingId) : IRequest<ReceivingDto?>;

public class GetReceivingQueryHandler : IRequestHandler<GetReceivingQuery, ReceivingDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetReceivingQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReceivingDto?> Handle(GetReceivingQuery request, CancellationToken cancellationToken)
    {
        var receiving = await _unitOfWork.Receivings.GetByIdAsync(request.ReceivingId, cancellationToken);
        return receiving is null ? null : _mapper.Map<ReceivingDto>(receiving);
    }
}
