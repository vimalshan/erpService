using MediatR;
using MemberService.Application.DTOs;

namespace MemberService.Application.Commands.AddNominee;

public record AddNomineeCommand(
    long MemberNo,
    int SerialNo,
    string FundType,
    string NomineeName,
    string RelationshipCode,
    long Percentage,
    DateTime DateOfBirth,
    bool IsMinor,
    string? AddressLine1,
    string? PhoneNo,
    string? Email,
    long CreatedBy
) : IRequest<NomineeDto>;
