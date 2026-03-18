using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Queries.Cheques;

public class ChequeQueryHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetChequeByIdQuery, ChequeDetailDto?>,
      IRequestHandler<GetChequesByStatusQuery, IReadOnlyList<ChequeDetailDto>>,
      IRequestHandler<GetAllChequesQuery, IReadOnlyList<ChequeDetailDto>>
{
    public async Task<ChequeDetailDto?> Handle(GetChequeByIdQuery request, CancellationToken cancellationToken)
    {
        var cheque = await unitOfWork.ChequeDetails.GetByIdAsync(request.ChequeId, cancellationToken);
        return cheque is null ? null : mapper.Map<ChequeDetailDto>(cheque);
    }

    public async Task<IReadOnlyList<ChequeDetailDto>> Handle(GetChequesByStatusQuery request, CancellationToken cancellationToken)
    {
        var cheques = await unitOfWork.ChequeDetails.GetByStatusAsync(request.Status, cancellationToken);
        return mapper.Map<IReadOnlyList<ChequeDetailDto>>(cheques);
    }

    public async Task<IReadOnlyList<ChequeDetailDto>> Handle(GetAllChequesQuery request, CancellationToken cancellationToken)
    {
        var cheques = await unitOfWork.ChequeDetails.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ChequeDetailDto>>(cheques);
    }
}
