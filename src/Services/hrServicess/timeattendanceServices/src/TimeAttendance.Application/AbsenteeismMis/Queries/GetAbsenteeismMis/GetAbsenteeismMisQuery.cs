using MediatR;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Application.AbsenteeismMis.Queries.GetAbsenteeismMis;

public record GetAbsenteeismMisQuery(long Id) : IRequest<AbsenteeismMisDto?>;
