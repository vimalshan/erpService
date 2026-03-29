using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetAll;

public record GetAllPreEmploymentCheckupsQuery : IRequest<IReadOnlyList<PreEmploymentCheckupDto>>;

public class GetAllPreEmploymentCheckupsQueryHandler : IRequestHandler<GetAllPreEmploymentCheckupsQuery, IReadOnlyList<PreEmploymentCheckupDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllPreEmploymentCheckupsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> Handle(GetAllPreEmploymentCheckupsQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.PreEmploymentCheckups.GetAllAsync(cancellationToken);
        return items.Select(CreatePreEmploymentCheckupCommandHandler.MapToDto).ToList();
    }
}
