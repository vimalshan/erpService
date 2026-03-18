using FinyearAPI.GraphQL.Types;

namespace FinyearAPI.GraphQL.Subscriptions
{
    /// <summary>
    /// GraphQL Subscription type for real-time financial year events
    /// </summary>
    public class FinancialYearSubscription
    {
        /// <summary>
        /// Subscribe to financial year created events
        /// </summary>
        public async IAsyncEnumerable<FinancialYearEventPayload> OnFinancialYearCreated()
        {
            // WebSocket subscription implementation
            yield return new FinancialYearEventPayload
            {
                EventType = "Created",
                OccurredAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Subscribe to financial year updated events
        /// </summary>
        public async IAsyncEnumerable<FinancialYearEventPayload> OnFinancialYearUpdated(long? id = null)
        {
            // WebSocket subscription implementation
            yield return new FinancialYearEventPayload
            {
                EventType = "Updated",
                OccurredAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Subscribe to financial year closed events
        /// </summary>
        public async IAsyncEnumerable<FinancialYearEventPayload> OnFinancialYearClosed()
        {
            // WebSocket subscription implementation
            yield return new FinancialYearEventPayload
            {
                EventType = "Closed",
                OccurredAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Subscribe to all financial year events
        /// </summary>
        public async IAsyncEnumerable<FinancialYearEventPayload> OnFinancialYearEvent()
        {
            // WebSocket subscription implementation
            yield return new FinancialYearEventPayload
            {
                EventType = "Event",
                OccurredAt = DateTime.UtcNow
            };
        }
    }
}
