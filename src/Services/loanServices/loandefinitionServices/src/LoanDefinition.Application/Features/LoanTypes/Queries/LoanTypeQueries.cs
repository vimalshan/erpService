using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.LoanTypes.Queries;

public record GetAllLoanTypesQuery : IRequest<IReadOnlyList<LoanTypeMasterDto>>;
public record GetLoanTypeByIdQuery(long LoanTypeId) : IRequest<LoanTypeMasterDto?>;
public record GetLoanTypesByCategoryQuery(string Category) : IRequest<IReadOnlyList<LoanTypeMasterDto>>;
