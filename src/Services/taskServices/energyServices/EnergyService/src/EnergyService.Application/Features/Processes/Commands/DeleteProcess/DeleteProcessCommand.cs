using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.DeleteProcess;

public record DeleteProcessCommand(int EcProcessId) : IRequest<bool>;
