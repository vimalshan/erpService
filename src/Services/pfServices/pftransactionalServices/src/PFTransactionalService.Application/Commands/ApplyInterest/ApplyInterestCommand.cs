using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Commands.ApplyInterest;

public record ApplyInterestCommand : IRequest<PFAccumulationDto>
{
    public long EmpSysId { get; init; }
    public decimal InterestAmount { get; init; }
    public long ProcessedBy { get; init; }
}
