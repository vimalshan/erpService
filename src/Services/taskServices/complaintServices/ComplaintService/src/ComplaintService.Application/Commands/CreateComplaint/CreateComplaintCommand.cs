using MediatR;

namespace ComplaintService.Application.Commands.CreateComplaint;

public record CreateComplaintCommand(
    decimal GroupId,
    decimal Type,
    decimal Location,
    decimal Department,
    decimal Process,
    string? Subject,
    string? Description,
    bool IsNCR,
    int TargetResolutionHours,
    decimal CreatedBy
) : IRequest<decimal>;
