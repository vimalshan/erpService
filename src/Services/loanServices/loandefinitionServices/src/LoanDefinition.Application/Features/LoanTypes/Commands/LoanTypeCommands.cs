using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.LoanTypes.Commands;

public record CreateLoanTypeCommand(long LoanType, string LoanName, string LoanCategory, long CreatedBy)
    : IRequest<LoanTypeMasterDto>;

public record UpdateLoanTypeCommand(long LoanType, string LoanName, string LoanCategory, long ModifiedBy)
    : IRequest<LoanTypeMasterDto>;

public record DeleteLoanTypeCommand(long LoanType) : IRequest<bool>;
