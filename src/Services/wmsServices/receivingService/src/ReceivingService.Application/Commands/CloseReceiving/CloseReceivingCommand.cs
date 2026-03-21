using MediatR;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Application.Commands.CloseReceiving;

public sealed record CloseReceivingCommand(int ReceivingId) : IRequest<ReceivingDto>;
