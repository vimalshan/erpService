using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByEmployeeNum;

public record GetPreEmploymentCheckupsByEmployeeNumQuery(decimal EmpNum) : IRequest<IReadOnlyList<PreEmploymentCheckupDto>>;

public class GetPreEmploymentCheckupsByEmployeeNumQueryHandler
    : IRequestHandler<GetPreEmploymentCheckupsByEmployeeNumQuery, IReadOnlyList<PreEmploymentCheckupDto>>
{
    private readonly IUnitOfWork _uow;
    public GetPreEmploymentCheckupsByEmployeeNumQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> Handle(GetPreEmploymentCheckupsByEmployeeNumQuery request, CancellationToken cancellationToken)
    {
        var items = await _uow.PreEmploymentCheckups.GetByEmployeeNumAsync(request.EmpNum, cancellationToken);
        return items.Select(CreatePreEmploymentCheckupCommandHandler.MapToDto).ToList();
    }
}
