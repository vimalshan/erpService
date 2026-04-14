using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CloseReceiving;

public record CloseReceivingCommand(int ReceivingId) : IRequest<ReceivingDto>;
