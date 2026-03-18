using MediatR;

namespace ScholarshipService.Application.Commands.ApproveScholarship;

public record ApproveScholarshipCommand(int ScholarshipId, int ApprovedBy, string? Remarks = null) : IRequest<bool>;
