using AutoMapper;
using MemberService.Application.DTOs;
using MemberService.Domain.Aggregates;
using MemberService.Domain.Entities;
using MemberService.Domain.Enums;

namespace MemberService.Application.Mappings;

public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        CreateMap<Member, MemberDto>()
            .ConstructUsing(m => new MemberDto(
                m.MemberNo, m.TrustCode, m.MemberName, m.FatherName,
                m.DateOfJoining, m.DateOfBirth, m.EmployeeType, m.UnitCode,
                m.EmployeeNo, m.EmployeeSysId,
                m.Status.ToString(), m.EnrollmentDate, m.ClosureDate, m.LeaveReason));

        CreateMap<Member, MemberSummaryDto>()
            .ConstructUsing(m => new MemberSummaryDto(
                m.MemberNo, m.MemberName, m.TrustCode, m.Status.ToString(),
                m.DateOfJoining, m.UnitCode,
                m.Nominees.Count(n => n.Status == NomineeStatus.Active)));

        CreateMap<MemberNominee, NomineeDto>()
            .ConstructUsing(n => new NomineeDto(
                n.SerialNo, n.FundType, n.NomineeName, n.RelationshipCode,
                n.Percentage, n.DateOfBirth, n.IsMinor, n.Status.ToString()));

        CreateMap<MemberContact, ContactDto>()
            .ConstructUsing(c => new ContactDto(
                c.ContactId, c.ContactType.ToString(), c.AddressLine1, c.AddressLine2,
                c.AddressLine3, c.City, c.State, c.PinCode, c.Country,
                c.PhoneNo, c.Email, c.EffectiveDate));
    }
}
