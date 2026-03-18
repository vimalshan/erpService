using AutoMapper;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Queries.GetAllReimbursements;

public sealed record GetAllReimbursementsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<IEnumerable<ReimbursementDto>>;

public sealed class GetAllReimbursementsQueryHandler(
    IReimbursementRepository repository,
    IMapper mapper) : IRequestHandler<GetAllReimbursementsQuery, IEnumerable<ReimbursementDto>>
{
    public async Task<IEnumerable<ReimbursementDto>> Handle(GetAllReimbursementsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        return mapper.Map<IEnumerable<ReimbursementDto>>(entities);
    }
}
