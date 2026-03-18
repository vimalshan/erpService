using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Repositories;
using MediatR;

namespace LoanDefinition.Application.Features.LoanTypes.Queries;

public class GetAllLoanTypesQueryHandler(ILoanTypeMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetAllLoanTypesQuery, IReadOnlyList<LoanTypeMasterDto>>
{
    public async Task<IReadOnlyList<LoanTypeMasterDto>> Handle(GetAllLoanTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<LoanTypeMasterDto>>(entities);
    }
}

public class GetLoanTypeByIdQueryHandler(ILoanTypeMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetLoanTypeByIdQuery, LoanTypeMasterDto?>
{
    public async Task<LoanTypeMasterDto?> Handle(GetLoanTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanTypeId, cancellationToken);
        return entity is null ? null : mapper.Map<LoanTypeMasterDto>(entity);
    }
}

public class GetLoanTypesByCategoryQueryHandler(ILoanTypeMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetLoanTypesByCategoryQuery, IReadOnlyList<LoanTypeMasterDto>>
{
    public async Task<IReadOnlyList<LoanTypeMasterDto>> Handle(GetLoanTypesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByCategoryAsync(request.Category, cancellationToken);
        return mapper.Map<IReadOnlyList<LoanTypeMasterDto>>(entities);
    }
}
