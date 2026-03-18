using AutoMapper;
using DeductionService.Application.DTOs;
using DeductionService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DeductionService.Application.CQRS.Queries.GetDeductionsByEmployee;

public record GetDeductionsByEmployeeQuery(long EmployeeNumber) : IRequest<IEnumerable<AdhocPayDeductionDto>>;

public class GetDeductionsByEmployeeQueryValidator : AbstractValidator<GetDeductionsByEmployeeQuery>
{
    public GetDeductionsByEmployeeQueryValidator()
    {
        RuleFor(x => x.EmployeeNumber).GreaterThan(0);
    }
}

public class GetDeductionsByEmployeeQueryHandler(
    IAdhocPayDeductionRepository repository,
    IMapper mapper)
    : IRequestHandler<GetDeductionsByEmployeeQuery, IEnumerable<AdhocPayDeductionDto>>
{
    public async Task<IEnumerable<AdhocPayDeductionDto>> Handle(
        GetDeductionsByEmployeeQuery request, CancellationToken ct)
    {
        var deductions = await repository.GetByEmployeeAsync(request.EmployeeNumber, ct);
        return mapper.Map<IEnumerable<AdhocPayDeductionDto>>(deductions);
    }
}
