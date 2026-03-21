using MediatR;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Application.Commands.CancelReceiving;

public sealed record CancelReceivingCommand(int ReceivingId) : IRequest<ReceivingDto>;
