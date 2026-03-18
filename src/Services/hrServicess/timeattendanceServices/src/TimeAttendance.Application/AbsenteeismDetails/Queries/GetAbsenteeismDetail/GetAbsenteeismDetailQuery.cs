using MediatR;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetail;

public record GetAbsenteeismDetailQuery(long Id) : IRequest<AbsenteeismDetailDto?>;
