using LocationServices.Application.DTOs;

namespace LocationServices.API.GraphQL.Types;

/// <summary>GraphQL Object Type — LocationAppMap</summary>
public sealed class LocationAppMapType : ObjectType<LocationAppMapDto>
{
    protected override void Configure(IObjectTypeDescriptor<LocationAppMapDto> descriptor)
    {
        descriptor.Description("Represents a mapping between a Location and an Application.");

        descriptor.Field(f => f.LocationId)
            .Description("The unique location identifier (decimal).");

        descriptor.Field(f => f.AppName)
            .Description("The application name associated with this location.");

        descriptor.Field(f => f.SiteCategoryCode)
            .Description("Optional site category code.");

        descriptor.Field(f => f.SelfAccess)
            .Description("Self-access flag value.");

        descriptor.Field(f => f.DeemedApproval)
            .Description("Whether deemed approval is applicable (Y/N).");

        descriptor.Field(f => f.IsActive)
            .Description("Whether this mapping is active.");

        descriptor.Field(f => f.CreatedDate)
            .Description("UTC timestamp when the mapping was created.");

        descriptor.Field(f => f.CreatedBy)
            .Description("User who created the mapping.");

        descriptor.Field(f => f.ModifiedDate)
            .Description("UTC timestamp of last modification.");

        descriptor.Field(f => f.ModifiedBy)
            .Description("User who last modified the mapping.");
    }
}
