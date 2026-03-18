using AutoMapper;
using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementById;

public sealed record GetReimbursementByIdQuery(long ReimId) : IRequest<ReimbursementDto?>;

public sealed class GetReimbursementByIdQueryHandler(
    IReimbursementRepository repository,
    IMapper mapper) : IRequestHandler<GetReimbursementByIdQuery, ReimbursementDto?>
{
    public async Task<ReimbursementDto?> Handle(GetReimbursementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken);
        return entity is null ? null : mapper.Map<ReimbursementDto>(entity);
    }
}
