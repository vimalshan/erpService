using LoanDefinition.Application.DTOs;
using LoanDefinition.Application.Features.Loans.Queries;
using LoanDefinition.Application.Features.LoanTypes.Queries;
using LoanDefinition.Application.Features.Festivals.Queries;
using LoanDefinition.Infrastructure.Dapper;
using MediatR;

namespace LoanDefinition.API.GraphQL;

public class LoanQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<LoanTypeMasterDto>> GetLoanTypes([Service] IMediator mediator)
        => await mediator.Send(new GetAllLoanTypesQuery());

    public async Task<LoanTypeMasterDto?> GetLoanTypeById([Service] IMediator mediator, long id)
        => await mediator.Send(new GetLoanTypeByIdQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<LoanMasterDto>> GetLoans([Service] IMediator mediator)
        => await mediator.Send(new GetAllLoansQuery());

    public async Task<LoanMasterDto?> GetLoanById([Service] IMediator mediator, long id)
        => await mediator.Send(new GetLoanByIdQuery(id));

    public async Task<LoanMasterDetailDto?> GetLoanDetail([Service] IMediator mediator, long id)
        => await mediator.Send(new GetLoanDetailQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<LoanMasterDto>> GetActiveLoans([Service] IMediator mediator)
        => await mediator.Send(new GetActiveLoansQuery());

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<LoanFestivalDto>> GetFestivals([Service] IMediator mediator)
        => await mediator.Send(new GetAllFestivalsQuery());

    public async Task<IEnumerable<LoanInterestRateDto>> GetInterestRates([Service] ILoanDapperQueries dapper, long loanId)
        => await dapper.GetInterestRatesByLoanIdAsync(loanId);

    public async Task<IEnumerable<LoanLimitRangeDto>> GetLimitRanges([Service] ILoanDapperQueries dapper, long loanId)
        => await dapper.GetLimitRangesByLoanIdAsync(loanId);
}
