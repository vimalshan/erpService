using System.Reflection;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace ProblemManagement.API.GraphQL;

/// <summary>
/// Removes the DomainEvents field (from BaseEntity) from all GraphQL object types
/// before HC registers dependencies, preventing it from discovering the empty IDomainEvent interface.
/// </summary>
public class IgnoreDomainEventsTypeInterceptor : TypeInterceptor
{
    public override void OnBeforeRegisterDependencies(
        ITypeDiscoveryContext discoveryContext,
        DefinitionBase definition)
    {
        if (definition is ObjectTypeDefinition objectDef)
        {
            for (int i = objectDef.Fields.Count - 1; i >= 0; i--)
            {
                var field = objectDef.Fields[i];
                if (field.Name == "domainEvents" ||
                    (field.Member is PropertyInfo pi && pi.Name == "DomainEvents"))
                {
                    objectDef.Fields.RemoveAt(i);
                }
            }
        }
    }
}
