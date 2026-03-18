using MediatR;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Mappings;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Application.Queries.GetPlans;

public class GetPlansQueryHandler : IRequestHandler<GetPlansQuery, IEnumerable<LetPlanDto>>
{
    private readonly ILetPlanRepository _repository;

    public GetPlansQueryHandler(ILetPlanRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<LetPlanDto>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _repository.GetAllAsync(request.UserId, request.Status, cancellationToken);
        return plans.ToDtos();
    }
}
