namespace RequestServices.Application.DTOs;

public record RequestMainDto(
    long   RequestId,
    string EmployeeUser,
    DateTime RequestDate,
    string SupervisorUser,
    IEnumerable<RequestSubDto> SubRequests
);

public record RequestSubDto(
    long   SerialNumber,
    string TrainingNeed,
    char   StatusCode,
    long   CourseId,
    string CourseDescription,
    DateTime StartDate,
    DateTime EndDate,
    string BusinessBenefit,
    string ExpectedCompetency,
    DateTime? CancellationDate,
    string? CancellationRemark
);

public record RequestAppDto(
    long   RequestId,
    long   SerialNumber,
    DateTime ApprovalDate,
    long   ApprovalNumber,
    string ApprovalRemark,
    string ApprovalUser
);

public record CreateRequestDto(
    long   RequestId,
    string EmployeeUser,
    DateTime RequestDate,
    string SupervisorUser,
    string TrainingNeed,
    long   CourseId,
    string CourseDescription,
    DateTime StartDate,
    DateTime EndDate,
    string BusinessBenefit,
    string ExpectedCompetency
);

public record ApproveRequestDto(
    long   RequestId,
    long   SerialNumber,
    long   ApprovalNumber,
    string ApprovalRemark,
    string ApprovalUser
);

public record CancelRequestDto(
    long   RequestId,
    long   SerialNumber,
    string Remark
);

public record PendingRequestDto(
    long   RequestId,
    string EmployeeUser,
    DateTime RequestDate,
    string TrainingNeed,
    char   StatusCode
);
