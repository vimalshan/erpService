using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetReceivings;

public record GetReceivingsQuery : IRequest<IEnumerable<ReceivingDto>>
{
    public int? PoId { get; init; }
    public string? Status { get; init; }
}

public class GetReceivingsQueryHandler : IRequestHandler<GetReceivingsQuery, IEnumerable<ReceivingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetReceivingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReceivingDto>> Handle(GetReceivingsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Receiving> receivings;

        if (request.PoId.HasValue)
            receivings = await _unitOfWork.Receivings.GetByPurchaseOrderAsync(request.PoId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.Status))
            receivings = await _unitOfWork.Receivings.GetByStatusAsync(request.Status, cancellationToken);
        else
            receivings = await _unitOfWork.Receivings.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ReceivingDto>>(receivings);
    }
}
