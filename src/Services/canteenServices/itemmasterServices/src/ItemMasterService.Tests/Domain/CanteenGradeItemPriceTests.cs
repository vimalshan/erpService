using FluentAssertions;
using ItemMasterService.Domain.Entities;
using Xunit;

namespace ItemMasterService.Tests.Domain;

public class CanteenGradeItemPriceTests
{
    // ── Create ───────────────────────────────────────────────────────────────

    [Fact]
    public void Create_WithValidInputs_SetsAllProperties()
    {
        var effectiveDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var closureDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001,
            itemCode: 5,
            employeeContribution: 20m,
            employerContribution: 40m,
            effectiveDate: effectiveDate,
            closureDate: closureDate,
            enteredBy: "admin",
            gradeType: "A");

        gradePrice.CanteenUnitCode.Should().Be(1001);
        gradePrice.ItemCode.Should().Be(5);
        gradePrice.EmployeeContribution.Should().Be(20m);
        gradePrice.EmployerContribution.Should().Be(40m);
        gradePrice.EffectiveDate.Should().Be(effectiveDate);
        gradePrice.ClosureDate.Should().Be(closureDate);
        gradePrice.EnteredBy.Should().Be("admin");
        gradePrice.GradeType.Should().Be("A");
        gradePrice.EnteredOn.Should().NotBeNull();
    }

    [Fact]
    public void Create_WithGradeTypeLongerThan3Chars_TruncatesToThreeChars()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: null,
            employeeContribution: 10m, employerContribution: 20m,
            effectiveDate: null, closureDate: DateTime.UtcNow,
            enteredBy: "admin", gradeType: "ABCD");

        gradePrice.GradeType.Should().HaveLength(3).And.Be("ABC");
    }

    [Fact]
    public void Create_WithNullItemCode_SetsItemCodeToNull()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: null,
            employeeContribution: 10m, employerContribution: 20m,
            effectiveDate: null, closureDate: DateTime.UtcNow,
            enteredBy: "admin", gradeType: "B");

        gradePrice.ItemCode.Should().BeNull();
    }

    [Fact]
    public void Create_WithNullEffectiveDate_SetsEffectiveDateToNull()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: 5,
            employeeContribution: 10m, employerContribution: 20m,
            effectiveDate: null, closureDate: DateTime.UtcNow,
            enteredBy: "admin", gradeType: "B");

        gradePrice.EffectiveDate.Should().BeNull();
    }

    [Fact]
    public void Create_WithEnteredByLongerThan50Chars_TruncatesToFiftyChars()
    {
        var longEnteredBy = new string('U', 60);

        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: null,
            employeeContribution: 10m, employerContribution: 20m,
            effectiveDate: null, closureDate: DateTime.UtcNow,
            enteredBy: longEnteredBy, gradeType: "B");

        gradePrice.EnteredBy.Should().HaveLength(50);
    }

    // ── Update ───────────────────────────────────────────────────────────────

    [Fact]
    public void Update_WithNewValues_UpdatesContributionsAndClosureDate()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: 5,
            employeeContribution: 20m, employerContribution: 40m,
            effectiveDate: null, closureDate: new DateTime(2024, 6, 30, 0, 0, 0, DateTimeKind.Utc),
            enteredBy: "admin", gradeType: "A");

        var newClosure = new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc);
        gradePrice.Update(30m, 60m, newClosure);

        gradePrice.EmployeeContribution.Should().Be(30m);
        gradePrice.EmployerContribution.Should().Be(60m);
        gradePrice.ClosureDate.Should().Be(newClosure);
    }

    [Fact]
    public void Update_SetsUpdatedAt()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: null,
            employeeContribution: 20m, employerContribution: 40m,
            effectiveDate: null, closureDate: DateTime.UtcNow.AddMonths(1),
            enteredBy: "admin", gradeType: "A");

        gradePrice.Update(30m, 60m, DateTime.UtcNow.AddMonths(2));

        gradePrice.UpdatedAt.Should().NotBeNull();
        gradePrice.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_IncrementsVersion()
    {
        var gradePrice = CanteenGradeItemPrice.Create(
            canteenUnitCode: 1001, itemCode: null,
            employeeContribution: 20m, employerContribution: 40m,
            effectiveDate: null, closureDate: DateTime.UtcNow.AddMonths(1),
            enteredBy: "admin", gradeType: "A");

        var versionBefore = gradePrice.Version;
        gradePrice.Update(30m, 60m, DateTime.UtcNow.AddMonths(2));

        gradePrice.Version.Should().Be(versionBefore + 1);
    }
}
