using EnergyService.Application.DTOs;

namespace EnergyService.API.GraphQL.Types;

public class EcReadingType : ObjectType<EcReadingDto>
{
    protected override void Configure(IObjectTypeDescriptor<EcReadingDto> descriptor)
    {
        descriptor.Name("EcReading");
        descriptor.Field(f => f.EbId).Description("Reading ID");
        descriptor.Field(f => f.EbUnitCode).Description("Unit code");
        descriptor.Field(f => f.EbProcessId).Description("Process ID");
        descriptor.Field(f => f.EbDate).Description("Reading date");
        descriptor.Field(f => f.EbTarget).Description("Target value");
        descriptor.Field(f => f.EbReading).Description("Meter reading value");
        descriptor.Field(f => f.EbActualUsage).Description("Calculated actual usage");
        descriptor.Field(f => f.EbRemarks).Description("Remarks");
    }
}
