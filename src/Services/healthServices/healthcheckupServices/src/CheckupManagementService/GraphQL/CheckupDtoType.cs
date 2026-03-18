using HotChocolate.Types;
using CheckupManagementService.DTOs;

namespace CheckupManagementService.GraphQL;

/// <summary>
/// GraphQL Object type for CheckupMasterDto
/// </summary>
public class CheckupMasterDtoType : ObjectType<CheckupMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<CheckupMasterDto> descriptor)
    {
        descriptor
            .Description("Represents a health checkup record");

        descriptor
            .Field(f => f.CheckupMasterId)
            .Description("Unique identifier for the checkup");

        descriptor
            .Field(f => f.EmployeeNumber)
            .Description("Employee ID associated with this checkup");

        descriptor
            .Field(f => f.CheckupDate)
            .Description("Date when the checkup was performed");

        descriptor
            .Field(f => f.CheckupType)
            .Description("Type of checkup performed");

        descriptor
            .Field(f => f.Status)
            .Description("Current status of the checkup");

        descriptor
            .Field(f => f.CreatedOn)
            .Description("Date when the checkup record was created");

        descriptor
            .Field(f => f.ApprovedDate)
            .Description("Date when the checkup was approved");

        descriptor
            .Field(f => f.DoctorCode)
            .Description("Doctor who performed the checkup");

        descriptor
            .Field(f => f.DoctorRemarks)
            .Description("Remarks from the doctor");

        descriptor
            .Field(f => f.ApprovedBy)
            .Description("User who approved the checkup");
    }
}

/// <summary>
/// GraphQL Object type for HealthMainDto
/// </summary>
public class HealthMainDtoType : ObjectType<HealthMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<HealthMainDto> descriptor)
    {
        descriptor
            .Description("Health examination and medical metrics");

        descriptor
            .Field(f => f.HealthId)
            .Description("Unique identifier for the health record");

        descriptor
            .Field(f => f.CheckupMasterId)
            .Description("Associated checkup master ID");

        descriptor
            .Field(f => f.EmployeeNumber)
            .Description("Employee ID");

        descriptor
            .Field(f => f.Height)
            .Description("Height in cm");

        descriptor
            .Field(f => f.Weight)
            .Description("Weight in kg");

        descriptor
            .Field(f => f.BMI)
            .Description("Body Mass Index");

        descriptor
            .Field(f => f.BloodPressure)
            .Description("Blood pressure reading");

        descriptor
            .Field(f => f.HeartRate)
            .Description("Heart rate in bpm");

        descriptor
            .Field(f => f.BloodGroup)
            .Description("Blood group");

        descriptor
            .Field(f => f.EyeVision)
            .Description("Vision status");

        descriptor
            .Field(f => f.OverallFitness)
            .Description("Overall fitness assessment");

        descriptor
            .Field(f => f.MedicalClearance)
            .Description("Medical clearance status");

        descriptor
            .Field(f => f.CreatedOn)
            .Description("Record creation date");
    }
}

/// <summary>
/// GraphQL Object type for TestMasterDto
/// </summary>
public class TestMasterDtoType : ObjectType<TestMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<TestMasterDto> descriptor)
    {
        descriptor
            .Description("Medical test master information");

        descriptor
            .Field(f => f.TestId)
            .Description("Test identifier");

        descriptor
            .Field(f => f.TestName)
            .Description("Name of the test");

        descriptor
            .Field(f => f.TestCategory)
            .Description("Category of the test");

        descriptor
            .Field(f => f.NormalRange)
            .Description("Normal range for the test");

        descriptor
            .Field(f => f.Unit)
            .Description("Unit of measurement");
    }
}
