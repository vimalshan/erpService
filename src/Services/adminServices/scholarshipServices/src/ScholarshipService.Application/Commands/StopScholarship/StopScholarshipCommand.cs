using MediatR;

namespace ScholarshipService.Application.Commands.StopScholarship;

public record StopScholarshipCommand(int ScholarshipId, string Reason, int StoppedBy) : IRequest<bool>;
