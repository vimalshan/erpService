using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Repositories;
using MediatR;

namespace LoanDefinition.Application.Features.Loans.Queries;

public class GetAllLoansQueryHandler(ILoanMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetAllLoansQuery, IReadOnlyList<LoanMasterDto>>
{
    public async Task<IReadOnlyList<LoanMasterDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<LoanMasterDto>>(entities);
    }
}

public class GetLoanByIdQueryHandler(ILoanMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetLoanByIdQuery, LoanMasterDto?>
{
    public async Task<LoanMasterDto?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanId, cancellationToken);
        return entity is null ? null : mapper.Map<LoanMasterDto>(entity);
    }
}

public class GetLoanDetailQueryHandler(ILoanMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetLoanDetailQuery, LoanMasterDetailDto?>
{
    public async Task<LoanMasterDetailDto?> Handle(GetLoanDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetWithDetailsAsync(request.LoanId, cancellationToken);
        if (entity is null) return null;

        return new LoanMasterDetailDto(
            mapper.Map<LoanMasterDto>(entity),
            mapper.Map<IReadOnlyList<LoanSubClassDto>>(entity.SubClasses),
            mapper.Map<IReadOnlyList<LoanInterestRateDto>>(entity.InterestRates),
            mapper.Map<IReadOnlyList<LoanLimitRangeDto>>(entity.LimitRanges),
            mapper.Map<IReadOnlyList<LoanFestivalMapDto>>(entity.FestivalMaps));
    }
}

public class GetLoansByTypeQueryHandler(ILoanMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetLoansByTypeQuery, IReadOnlyList<LoanMasterDto>>
{
    public async Task<IReadOnlyList<LoanMasterDto>> Handle(GetLoansByTypeQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByTypeAsync(request.LoanTypeId, cancellationToken);
        return mapper.Map<IReadOnlyList<LoanMasterDto>>(entities);
    }
}

public class GetActiveLoansQueryHandler(ILoanMasterRepository repository, IMapper mapper)
    : IRequestHandler<GetActiveLoansQuery, IReadOnlyList<LoanMasterDto>>
{
    public async Task<IReadOnlyList<LoanMasterDto>> Handle(GetActiveLoansQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetActiveLoansAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<LoanMasterDto>>(entities);
    }
}
