using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemberService.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Seeds representative demo data:
    ///   - 3 members (active new-joiner, active internal-transfer with minor nominee, closed)
    ///   - Payroll, contacts, GPF+FPS nominees, guardian for minor, and audit logs
    /// </summary>
    public partial class SeedData : Migration
    {
        private static readonly string[] MasterCols =
        [
            "MEMBER_NO", "MEMBER_TRUST_CODE", "MEMBER_FPSTRUST_CODE",
            "MEMBER_OPF_NO", "MEMBER_PENSION_NO",
            "MEMBER_NAME", "MEMBER_FATHERNAME", "MEMBER_DOB",
            "MEMBER_ENR_DATE", "MEMBER_DOJ", "MEMBER_EMPLOYEE_TYPE",
            "MEMBER_UNIT_CODE", "MEMBER_EMP_NUM", "MEMBER_EMP_SYSID",
            "MEMBER_ENROLL_USER_ID", "MEMBER_ENROLL_SYSID", "MEMBER_ENROLL_DATE",
            "MEMBER_STATUS",
            "MEMBER_CLOSURE_DATE", "MEMBER_LEAVE_DATE", "MEMBER_LEAVE_REASON",
            "MEMBER_UPDATED_BY", "MEMBER_UPDATED_ON"
        ];

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── MEMBER_MASTER ────────────────────────────────────────────────────

            // 1001 – John Doe, Active, New employee (N)
            migrationBuilder.InsertData("MEMBER_MASTER", MasterCols, new object[]
            {
                1001L, "SRF", "SRF", 1001, 1001,
                "JOHN DOE", "JAMES DOE", new DateTime(1985, 6, 10),
                new DateTime(2020, 1, 15), new DateTime(2020, 1, 15), "N",
                "SRF", 5001L, 1001L,
                "ADMIN", 1L, new DateTime(2020, 1, 15),
                "A",
                null, null, null,
                1L, new DateTime(2020, 1, 15)
            });

            // 1002 – Priya Sharma, Active, Internal transfer (S)
            migrationBuilder.InsertData("MEMBER_MASTER", MasterCols, new object[]
            {
                1002L, "SRF", "SRF", 1002, 1002,
                "PRIYA SHARMA", "RAJESH SHARMA", new DateTime(1990, 11, 25),
                new DateTime(2018, 4, 1), new DateTime(2018, 4, 1), "S",
                "SRF", 5002L, 1002L,
                "ADMIN", 1L, new DateTime(2018, 4, 1),
                "A",
                null, null, null,
                1L, new DateTime(2018, 4, 1)
            });

            // 1003 – Ravi Kumar, Closed (C), External transfer (O)
            migrationBuilder.InsertData("MEMBER_MASTER", MasterCols, new object[]
            {
                1003L, "SRF", "SRF", 1003, 1003,
                "RAVI KUMAR", "SURESH KUMAR", new DateTime(1978, 3, 14),
                new DateTime(2015, 6, 10), new DateTime(2015, 6, 10), "O",
                "SRF", 5003L, 1003L,
                "ADMIN", 1L, new DateTime(2015, 6, 10),
                "C",
                new DateTime(2024, 12, 31), new DateTime(2024, 12, 31), "VOLUNTARY RETIREMENT",
                1L, new DateTime(2024, 12, 31)
            });

            // ── MEMBER_PAYROLL ───────────────────────────────────────────────────

            migrationBuilder.InsertData("MEMBER_PAYROLL",
                ["PAYROLL_MEMBER_NO", "PAYROLL_UNT_COD", "PAYROLL_EMP_NUM", "PAYROLL_EFF_DATE", "PAYROLL_STATUS"],
                new object[] { 1001L, "SRF", 5001L, new DateTime(2020, 1, 15), "A" });

            migrationBuilder.InsertData("MEMBER_PAYROLL",
                ["PAYROLL_MEMBER_NO", "PAYROLL_UNT_COD", "PAYROLL_EMP_NUM", "PAYROLL_EFF_DATE", "PAYROLL_STATUS"],
                new object[] { 1002L, "SRF", 5002L, new DateTime(2018, 4, 1), "A" });

            migrationBuilder.InsertData("MEMBER_PAYROLL",
                ["PAYROLL_MEMBER_NO", "PAYROLL_UNT_COD", "PAYROLL_EMP_NUM",
                 "PAYROLL_EFF_DATE", "PAYROLL_CLS_DATE", "PAYROLL_STATUS"],
                new object[]
                {
                    1003L, "SRF", 5003L,
                    new DateTime(2015, 6, 10), new DateTime(2024, 12, 31), "C"
                });

            // ── MEMBER_NOMINEE ───────────────────────────────────────────────────

            string[] nomineeCols =
            [
                "NOMINEE_MEMBER_NO", "NOMINEE_SERIAL_NO", "NOMINEE_FUND_TYPE",
                "NOMINEE_NAME", "NOMINEE_RELATIONSHIP_CODE", "NOMINEE_PERCENTAGE",
                "NOMINEE_DOB", "NOMINEE_EFF_DATE", "NOMINEE_MINOR_FLAG",
                "NOMINEE_TRUST_CODE", "NOMINEE_STATUS"
            ];

            // John Doe – GPF: spouse (100%)
            migrationBuilder.InsertData("MEMBER_NOMINEE", nomineeCols, new object[]
            {
                1001L, 1, "GPF", "JANE DOE", "SPO", 100L,
                new DateTime(1988, 3, 22), new DateTime(2020, 1, 15), "N", "SRF", "A"
            });

            // John Doe – FPS: spouse (100%)
            migrationBuilder.InsertData("MEMBER_NOMINEE", nomineeCols, new object[]
            {
                1001L, 1, "FPS", "JANE DOE", "SPO", 100L,
                new DateTime(1988, 3, 22), new DateTime(2020, 1, 15), "N", "SRF", "A"
            });

            // Priya Sharma – GPF: minor son (60%)
            migrationBuilder.InsertData("MEMBER_NOMINEE", nomineeCols, new object[]
            {
                1002L, 1, "GPF", "ARJUN SHARMA", "SON", 60L,
                new DateTime(2015, 7, 5), new DateTime(2018, 4, 1), "Y", "SRF", "A"
            });

            // Priya Sharma – GPF: mother (40%)
            migrationBuilder.InsertData("MEMBER_NOMINEE", nomineeCols, new object[]
            {
                1002L, 2, "GPF", "SUNITA SHARMA", "MTH", 40L,
                new DateTime(1962, 2, 18), new DateTime(2018, 4, 1), "N", "SRF", "A"
            });

            // Priya Sharma – FPS: minor son (100%)
            migrationBuilder.InsertData("MEMBER_NOMINEE", nomineeCols, new object[]
            {
                1002L, 1, "FPS", "ARJUN SHARMA", "SON", 100L,
                new DateTime(2015, 7, 5), new DateTime(2018, 4, 1), "Y", "SRF", "A"
            });

            // Ravi Kumar – GPF: spouse (closed)
            migrationBuilder.InsertData("MEMBER_NOMINEE",
                [
                    "NOMINEE_MEMBER_NO", "NOMINEE_SERIAL_NO", "NOMINEE_FUND_TYPE",
                    "NOMINEE_NAME", "NOMINEE_RELATIONSHIP_CODE", "NOMINEE_PERCENTAGE",
                    "NOMINEE_DOB", "NOMINEE_EFF_DATE", "NOMINEE_CLS_DATE",
                    "NOMINEE_MINOR_FLAG", "NOMINEE_TRUST_CODE", "NOMINEE_STATUS"
                ],
                new object[]
                {
                    1003L, 1, "GPF", "MEENA KUMAR", "SPO", 100L,
                    new DateTime(1980, 9, 3), new DateTime(2015, 6, 10), new DateTime(2024, 12, 31),
                    "N", "SRF", "I"
                });

            // ── NOMINEE_GAURDIAN (Priya's minor son Arjun) ──────────────────────

            migrationBuilder.InsertData("NOMINEE_GAURDIAN",
                [
                    "GN_TRUST_CODE", "GN_NOMINEE_MEMBER_NO", "GN_NOMINEE_SERIAL_NO",
                    "GAURDIAN_NAME", "GAURDIAN_RELATIONSHIP",
                    "GN_ADDRESS_LINE1", "GN_ADDRESS_LINE2", "GN_ADDRESS_LINE3",
                    "GN_PHONE_NO", "GN_EMAIL", "GN_EFF_DATE"
                ],
                new object[]
                {
                    "SRF", 1002L, 1L,
                    "RAJESH SHARMA", "FAT",
                    "12 GREEN PARK", "NEW DELHI", "DELHI - 110016",
                    "9911002200", "rajesh.sharma@example.com",
                    new DateTime(2018, 4, 1)
                });

            // ── MEMBER_CONTACT ───────────────────────────────────────────────────

            string[] contactCols =
            [
                "MEMBER_NO", "CONTACT_TYPE",
                "ADDRESS_LINE_1", "ADDRESS_LINE_2", "CITY", "STATE", "PIN_CODE", "COUNTRY",
                "PHONE_NO", "EMAIL", "EFF_DATE"
            ];

            // John Doe – permanent address
            migrationBuilder.InsertData("MEMBER_CONTACT", contactCols, new object[]
            {
                1001L, "P",
                "45 MAPLE STREET", "SECTOR 12", "MUMBAI", "MAHARASHTRA", "400001", "INDIA",
                "9876543210", "john.doe@example.com", new DateTime(2020, 1, 15)
            });

            // Priya Sharma – permanent address
            migrationBuilder.InsertData("MEMBER_CONTACT", contactCols, new object[]
            {
                1002L, "P",
                "12 GREEN PARK", "NEW DELHI", "NEW DELHI", "DELHI", "110016", "INDIA",
                "9911223344", "priya.sharma@example.com", new DateTime(2018, 4, 1)
            });

            // ── MEMBER_AUDIT_LOG (closure events for Ravi Kumar) ─────────────────

            string[] auditCols =
            [
                "MEMBER_NO", "AUDIT_ACTION", "AUDIT_TIMESTAMP",
                "AUDIT_USER_ID", "AUDIT_OLD_VALUES", "AUDIT_NEW_VALUES"
            ];

            migrationBuilder.InsertData("MEMBER_AUDIT_LOG", auditCols, new object[]
            {
                1003L, "MEMBER_CLOSED", new DateTime(2024, 12, 31, 17, 0, 0),
                1L,
                "{\"Status\":\"A\"}",
                "{\"Status\":\"C\",\"ClosureDate\":\"2024-12-31\",\"LeaveReason\":\"VOLUNTARY RETIREMENT\"}"
            });

            migrationBuilder.InsertData("MEMBER_AUDIT_LOG", auditCols, new object[]
            {
                1003L, "NOMINEE_DEACTIVATED", new DateTime(2024, 12, 31, 17, 5, 0),
                1L,
                "{\"NomineeStatus\":\"A\"}",
                "{\"NomineeStatus\":\"I\",\"ClosureDate\":\"2024-12-31\"}"
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MEMBER_AUDIT_LOG WHERE MEMBER_NO IN (1001, 1002, 1003)");
            migrationBuilder.Sql("DELETE FROM MEMBER_CONTACT       WHERE MEMBER_NO IN (1001, 1002, 1003)");
            migrationBuilder.Sql("DELETE FROM NOMINEE_GAURDIAN     WHERE GN_NOMINEE_MEMBER_NO IN (1002)");
            migrationBuilder.Sql("DELETE FROM MEMBER_NOMINEE       WHERE NOMINEE_MEMBER_NO IN (1001, 1002, 1003)");
            migrationBuilder.Sql("DELETE FROM MEMBER_PAYROLL       WHERE PAYROLL_MEMBER_NO IN (1001, 1002, 1003)");
            migrationBuilder.Sql("DELETE FROM MEMBER_MASTER        WHERE MEMBER_NO IN (1001, 1002, 1003)");
        }
    }
}

