using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Commands.ProcessContribution;

public record ProcessContributionCommand : IRequest<PFAccumulationDto>
{
    public long EmpSysId { get; init; }
    public long MemberNo { get; init; }
    public string TrustCode { get; init; } = string.Empty;
    public decimal EmpContribution { get; init; }
    public decimal ErContribution { get; init; }
    public decimal VolContribution { get; init; }
    public DateTime TxnMonth { get; init; }
    public long ProcessedBy { get; init; }
}
