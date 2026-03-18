using MediatR;
using MemberService.Application.DTOs;

namespace MemberService.Application.Commands.CreateMember;

public record CreateMemberCommand(
    string MemberName,
    string? FatherName,
    string TrustCode,
    DateTime DateOfJoining,
    DateTime? DateOfBirth,
    string EmployeeType,
    long EmployeeSysId,
    string UnitCode,
    long EmployeeNo,
    long CreatedBy
) : IRequest<MemberDto>;
