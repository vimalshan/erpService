using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Commands.GenerateCertificate;

public record GenerateCertificateCommand : IRequest<WithdrawalCertificateDto>
{
    public long SettlementId { get; init; }
    public long GeneratedBy { get; init; }
}
