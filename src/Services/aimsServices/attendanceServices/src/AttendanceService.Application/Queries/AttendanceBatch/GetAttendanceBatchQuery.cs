using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Queries.AttendanceBatch;

public record GetAttendanceBatchQuery(long BatchId) : IRequest<AttendanceBatchDto?>;
