using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.ProcessMail.Queries.GetMailIdsByProcess;

public record GetMailIdsByProcessQuery(int ProcessId) : IRequest<IReadOnlyList<EcProcessMailIdDto>>;
