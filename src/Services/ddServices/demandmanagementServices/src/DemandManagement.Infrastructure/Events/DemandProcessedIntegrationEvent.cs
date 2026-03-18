namespace DemandManagement.Infrastructure.Events;

public record DemandProcessedIntegrationEvent(long DemandId, string Status);
