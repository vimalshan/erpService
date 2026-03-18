using CourseService.Application.DTOs;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Participants.Queries.GetParticipants;

public class GetParticipantsQueryHandler(ICourseParticipantRepository repository) : IRequestHandler<GetParticipantsQuery, IEnumerable<CourseParticipantDto>>
{
    public async Task<IEnumerable<CourseParticipantDto>> Handle(GetParticipantsQuery query, CancellationToken ct)
    {
        var participants = await repository.GetByCourseIdAsync(query.CourseId, ct);
        return participants.Select(p => new CourseParticipantDto(
            p.CourseId, p.UserCode, p.NominationStatus, p.EnrollmentDate,
            p.ApprovalStatus, p.CancellationDate, p.CancellationRemark,
            p.AttendanceStatus, p.UserPin, p.ApproverCode, p.ApproverPin));
    }
}
