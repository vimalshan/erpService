using AutoMapper;
using DeductionService.Application.DTOs;
using DeductionService.Domain.Exceptions;
using DeductionService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DeductionService.Application.CQRS.Queries.GetDeductionById;

public record GetDeductionByIdQuery(long SystemId) : IRequest<AdhocPayDeductionDto>;

public class GetDeductionByIdQueryValidator : AbstractValidator<GetDeductionByIdQuery>
{
    public GetDeductionByIdQueryValidator()
    {
        RuleFor(x => x.SystemId).GreaterThan(0);
    }
}

public class GetDeductionByIdQueryHandler(
    IAdhocPayDeductionRepository repository,
    IMapper mapper)
    : IRequestHandler<GetDeductionByIdQuery, AdhocPayDeductionDto>
{
    public async Task<AdhocPayDeductionDto> Handle(GetDeductionByIdQuery request, CancellationToken ct)
    {
        var deduction = await repository.GetByIdAsync(request.SystemId, ct)
            ?? throw new DeductionDomainException($"Deduction {request.SystemId} not found.");

        return mapper.Map<AdhocPayDeductionDto>(deduction);
    }
}
