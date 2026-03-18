using MediatR;
using RequestServices.Application.DTOs;

namespace RequestServices.Application.Commands.CreateRequest;

public record CreateRequestCommand(
    long     RequestId,
    string   EmployeeUser,
    DateTime RequestDate,
    string   SupervisorUser,
    string   TrainingNeed,
    long     CourseId,
    string   CourseDescription,
    DateTime StartDate,
    DateTime EndDate,
    string   BusinessBenefit,
    string   ExpectedCompetency
) : IRequest<RequestMainDto>;
