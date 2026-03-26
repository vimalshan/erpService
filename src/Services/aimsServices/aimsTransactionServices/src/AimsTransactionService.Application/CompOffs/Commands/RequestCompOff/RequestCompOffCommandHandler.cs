using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;

public sealed class RequestCompOffCommandHandler(
    ICompOffRepository compOffRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RequestCompOffCommand, CompOffDto>
{
    public async Task<CompOffDto> Handle(RequestCompOffCommand request, CancellationToken cancellationToken)
    {
        var id = await compOffRepository.GetNextIdAsync(cancellationToken);

        var compOff = CompOffAggregate.Request(
            id,
            request.EmployeeSysId,
            request.HoursRequested,
            request.RequestedBy);

        await compOffRepository.AddAsync(compOff, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(compOff);
    }

    private static CompOffDto MapToDto(CompOffAggregate c) => new(
        c.Id,
        c.EmployeeSysId,
        c.RequestedOn,
        c.HoursRequested,
        c.Status.ToString(),
        c.RequestedBy,
        c.RequestedOn);
}
