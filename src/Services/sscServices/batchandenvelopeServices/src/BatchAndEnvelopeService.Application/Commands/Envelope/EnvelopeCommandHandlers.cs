using MediatR;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Interfaces;

namespace BatchAndEnvelopeService.Application.Commands.Envelope;

public class CreateEnvelopeCommandHandler : IRequestHandler<CreateEnvelopeCommand, EnvelopeDto>
{
    private readonly IEnvelopeRepository _envelopeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnvelopeCommandHandler(IEnvelopeRepository envelopeRepository, IUnitOfWork unitOfWork)
    {
        _envelopeRepository = envelopeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EnvelopeDto> Handle(CreateEnvelopeCommand request, CancellationToken cancellationToken)
    {
        var id = await _envelopeRepository.GetNextIdAsync(cancellationToken);
        var envelope = EnvelopeAggregate.Create(id, request.EnvelopeType, request.CreatedBy, request.LocationId);

        long detId = 1;
        foreach (var (docId, type) in request.Documents)
        {
            var detail = EnvelopeDetail.Create(detId++, id, type, docId, request.CreatedBy);
            envelope.AddDetail(detail);
        }

        await _envelopeRepository.AddAsync(envelope, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(envelope);
    }

    private static EnvelopeDto MapToDto(EnvelopeAggregate e) => new(
        e.Id, e.EnvelopeType, e.CreatedBy, e.CreatedOn, e.ReceivedBy, e.ReceivedOn,
        e.SummaryFlag, e.CancelledBy, e.CancelledOn, e.ConfirmedBy, e.ConfirmedOn,
        e.ScanLotNo, e.LocationId,
        e.Details.Select(d => new EnvelopeDetailDto(d.Id, d.EnvelopeId, d.EnvelopeType, d.DocumentId, d.CreatedBy, d.CreatedOn, d.ReceiveFlag))
    );
}

public class ConfirmEnvelopeCommandHandler : IRequestHandler<ConfirmEnvelopeCommand, EnvelopeDto>
{
    private readonly IEnvelopeRepository _envelopeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEnvelopeCommandHandler(IEnvelopeRepository envelopeRepository, IUnitOfWork unitOfWork)
    {
        _envelopeRepository = envelopeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EnvelopeDto> Handle(ConfirmEnvelopeCommand request, CancellationToken cancellationToken)
    {
        var envelope = await _envelopeRepository.GetByIdAsync(request.EnvelopeId, cancellationToken)
            ?? throw new Domain.Exceptions.EnvelopeNotFoundException(request.EnvelopeId);

        envelope.Confirm(request.ConfirmedBy);
        await _envelopeRepository.UpdateAsync(envelope, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EnvelopeDto(envelope.Id, envelope.EnvelopeType, envelope.CreatedBy, envelope.CreatedOn,
            envelope.ReceivedBy, envelope.ReceivedOn, envelope.SummaryFlag, envelope.CancelledBy,
            envelope.CancelledOn, envelope.ConfirmedBy, envelope.ConfirmedOn, envelope.ScanLotNo, envelope.LocationId, []);
    }
}

public class CancelEnvelopeCommandHandler : IRequestHandler<CancelEnvelopeCommand, EnvelopeDto>
{
    private readonly IEnvelopeRepository _envelopeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEnvelopeCommandHandler(IEnvelopeRepository envelopeRepository, IUnitOfWork unitOfWork)
    {
        _envelopeRepository = envelopeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EnvelopeDto> Handle(CancelEnvelopeCommand request, CancellationToken cancellationToken)
    {
        var envelope = await _envelopeRepository.GetByIdAsync(request.EnvelopeId, cancellationToken)
            ?? throw new Domain.Exceptions.EnvelopeNotFoundException(request.EnvelopeId);

        envelope.Cancel(request.CancelledBy);
        await _envelopeRepository.UpdateAsync(envelope, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EnvelopeDto(envelope.Id, envelope.EnvelopeType, envelope.CreatedBy, envelope.CreatedOn,
            envelope.ReceivedBy, envelope.ReceivedOn, envelope.SummaryFlag, envelope.CancelledBy,
            envelope.CancelledOn, envelope.ConfirmedBy, envelope.ConfirmedOn, envelope.ScanLotNo, envelope.LocationId, []);
    }
}
