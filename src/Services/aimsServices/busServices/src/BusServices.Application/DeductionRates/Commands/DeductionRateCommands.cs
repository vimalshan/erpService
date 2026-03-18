using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Entities;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.DeductionRates.Commands;

public record SetDeductionRateCommand(
    int BusId,
    decimal Amount,
    DateTime EffectiveDate,
    long CreatedBy) : IRequest<BusDeductionRateDto>;

public sealed class SetDeductionRateCommandHandler : IRequestHandler<SetDeductionRateCommand, BusDeductionRateDto>
{
    private readonly IBusRepository _busRepo;
    private readonly IBusDeductionRateRepository _rateRepo;

    public SetDeductionRateCommandHandler(IBusRepository busRepo, IBusDeductionRateRepository rateRepo)
    {
        _busRepo = busRepo;
        _rateRepo = rateRepo;
    }

    public async Task<BusDeductionRateDto> Handle(SetDeductionRateCommand request, CancellationToken ct)
    {
        if (!await _busRepo.ExistsAsync(request.BusId, ct))
            throw new KeyNotFoundException($"Bus {request.BusId} not found.");

        int nextId = await _rateRepo.GetNextIdAsync(ct);
        var rate = BusDeductionRate.Create(nextId, request.BusId, request.Amount, request.EffectiveDate, request.CreatedBy);

        await _rateRepo.AddAsync(rate, ct);
        await _rateRepo.SaveChangesAsync(ct);

        return new BusDeductionRateDto(rate.DeductId, rate.BusId, rate.Amount,
            rate.EffectiveDate, rate.ClosingDate, rate.LastModifiedBy, rate.LastModifiedOn);
    }
}
