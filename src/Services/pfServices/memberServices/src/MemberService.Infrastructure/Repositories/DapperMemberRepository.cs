using System.Data;
using Dapper;
using MemberService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MemberService.Infrastructure.Repositories;

/// <summary>
/// Read-side Dapper repository for high-performance queries (CQRS read projection).
/// </summary>
public class DapperMemberRepository
{
    private readonly string _connectionString;

    public DapperMemberRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string is missing.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<MemberProfileDto?> GetMemberProfileAsync(long memberNo)
    {
        const string memberSql = @"
            SELECT MEMBER_NO AS MemberNo, MEMBER_NAME AS MemberName, MEMBER_TRUST_CODE AS TrustCode,
                   MEMBER_DOJ AS DateOfJoining, MEMBER_DOB AS DateOfBirth, MEMBER_STATUS AS Status,
                   MEMBER_EMP_SYSID AS EmployeeSysId, MEMBER_UNIT_CODE AS UnitCode, MEMBER_EMP_NUM AS EmployeeNo,
                   MEMBER_EMPLOYEE_TYPE AS EmployeeType, MEMBER_ENR_DATE AS EnrollmentDate,
                   MEMBER_CLOSURE_DATE AS ClosureDate, MEMBER_LEAVE_REASON AS LeaveReason,
                   MEMBER_FATHERNAME AS FatherName
            FROM MEMBER_MASTER WHERE MEMBER_NO = @MemberNo";

        const string nomineesSql = @"
            SELECT NOMINEE_SERIAL_NO AS SerialNo, NOMINEE_FUND_TYPE AS FundType, NOMINEE_NAME AS NomineeName,
                   NOMINEE_RELATIONSHIP_CODE AS RelationshipCode, NOMINEE_PERCENTAGE AS Percentage,
                   NOMINEE_DOB AS DateOfBirth, NOMINEE_MINOR_FLAG AS IsMinor, NOMINEE_STATUS AS Status
            FROM MEMBER_NOMINEE WHERE NOMINEE_MEMBER_NO = @MemberNo AND NOMINEE_STATUS = 'A'
            ORDER BY NOMINEE_SERIAL_NO";

        const string contactsSql = @"
            SELECT CONTACT_ID AS ContactId, CONTACT_TYPE AS ContactType, ADDRESS_LINE_1 AS AddressLine1,
                   ADDRESS_LINE_2 AS AddressLine2, ADDRESS_LINE_3 AS AddressLine3,
                   CITY, STATE, PIN_CODE AS PinCode, COUNTRY, PHONE_NO AS PhoneNo, EMAIL As Email,
                   EFF_DATE AS EffectiveDate
            FROM MEMBER_CONTACT WHERE MEMBER_NO = @MemberNo AND (CLS_DATE IS NULL OR CLS_DATE > GETDATE())";

        using var conn = CreateConnection();
        var member = await conn.QuerySingleOrDefaultAsync<dynamic>(memberSql, new { MemberNo = memberNo });
        if (member is null) return null;

        var nominees = (await conn.QueryAsync<dynamic>(nomineesSql, new { MemberNo = memberNo })).ToList();
        var contacts = (await conn.QueryAsync<dynamic>(contactsSql, new { MemberNo = memberNo })).ToList();

        var memberDto = new MemberDto(
            (long)member.MemberNo, (string)member.TrustCode, (string)member.MemberName,
            (string?)member.FatherName, (DateTime)member.DateOfJoining, (DateTime?)member.DateOfBirth,
            (string)member.EmployeeType, (string)member.UnitCode, (long)member.EmployeeNo,
            (long)member.EmployeeSysId, (string)member.Status, (DateTime)member.EnrollmentDate,
            (DateTime?)member.ClosureDate, (string?)member.LeaveReason);

        var nomineeDtos = nominees.Select(n => new NomineeDto(
            (int)n.SerialNo, (string)n.FundType, (string)n.NomineeName,
            (string)n.RelationshipCode, (long)n.Percentage, (DateTime)n.DateOfBirth,
            (string)n.IsMinor == "Y", (string)n.Status)).ToList();

        var contactDtos = contacts.Select(c => new ContactDto(
            (long)c.ContactId, (string)c.ContactType, (string)c.AddressLine1, (string?)c.AddressLine2,
            (string?)c.AddressLine3, (string)c.City, (string)c.State, (string)c.PinCode,
            (string)c.Country, (string?)c.PhoneNo, (string?)c.Email, (DateTime)c.EffectiveDate)).ToList();

        return new MemberProfileDto(memberDto, nomineeDtos, contactDtos);
    }

    public async Task<IReadOnlyList<MemberSummaryDto>> GetActiveMembersSummaryAsync(string? trustCode = null)
    {
        var sql = @"
            SELECT mm.MEMBER_NO AS MemberNo, mm.MEMBER_NAME AS MemberName, mm.MEMBER_TRUST_CODE AS TrustCode,
                   mm.MEMBER_STATUS AS Status, mm.MEMBER_DOJ AS DateOfJoining, mm.MEMBER_UNIT_CODE AS UnitCode,
                   COUNT(mn.NOMINEE_SERIAL_NO) AS NomineeCount
            FROM MEMBER_MASTER mm
            LEFT JOIN MEMBER_NOMINEE mn ON mm.MEMBER_NO = mn.NOMINEE_MEMBER_NO AND mn.NOMINEE_STATUS = 'A'
            WHERE mm.MEMBER_STATUS = 'A'" +
            (trustCode != null ? " AND mm.MEMBER_TRUST_CODE = @TrustCode" : "") +
            @" GROUP BY mm.MEMBER_NO, mm.MEMBER_NAME, mm.MEMBER_TRUST_CODE,
                       mm.MEMBER_STATUS, mm.MEMBER_DOJ, mm.MEMBER_UNIT_CODE";

        using var conn = CreateConnection();
        var results = await conn.QueryAsync<MemberSummaryDto>(sql, new { TrustCode = trustCode });
        return results.ToList();
    }
}
