using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Commands.AddInterest;

public record AddInterestCommand(
    decimal LoanId,
    string RateType,   // FX or FL
    decimal Percentage,
    long? FloatTypeId,
    DateTime EffectiveDate
) : IRequest<InterestDto>;
