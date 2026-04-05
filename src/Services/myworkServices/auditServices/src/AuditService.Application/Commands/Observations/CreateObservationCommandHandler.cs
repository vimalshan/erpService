using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands.Observations;

public sealed class CreateObservationCommandHandler : IRequestHandler<CreateObservationCommand, ObservationDto>
{
    private readonly IObservationRepository _observationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateObservationCommandHandler(IObservationRepository observationRepository, IUnitOfWork unitOfWork)
    {
        _observationRepository = observationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ObservationDto> Handle(CreateObservationCommand request, CancellationToken cancellationToken)
    {
        var observation = AuditObservation.Create(
            request.ObvId, request.AuditId, request.Title, request.Description,
            request.Risk[0], request.Auditee, request.Esc1, request.Esc2,
            request.ManComments, request.OrgDueDate, request.Location,
            request.AuditorName, request.Remarks, request.CreatedBy);

        await _observationRepository.AddAsync(observation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(observation);
    }

    private static ObservationDto ToDto(AuditObservation o) => new(
        o.ObvId, o.ObvAuditId, o.ObvTitle, o.ObvDescription, o.ObvRisk.ToString(),
        o.ObvAuditee, o.ObvEsc1, o.ObvEsc2, o.ObvManComments, o.ObvImplication,
        o.ObvStatus.ToString(), o.ObvOrgDueDate, o.ObvOrgRev1Date, o.ObvOrgRev2Date,
        o.ObvCreatedBy, o.ObvCreatedOn, o.ObvLocation, o.ObvAuditorName,
        o.ObvRemarks, o.ObvAppStatus?.ToString());
}
