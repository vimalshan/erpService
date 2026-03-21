using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.ProcessMail.Commands.ConfigureMailId;

public record ConfigureMailIdCommand(
    int ProcessId,
    string MailId,
    string DeliveryType,
    DateTime StartDate,
    DateTime? CloseDate,
    int ModifiedBy) : IRequest<EcProcessMailIdDto>;
