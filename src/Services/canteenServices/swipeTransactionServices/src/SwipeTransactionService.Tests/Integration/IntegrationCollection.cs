using Xunit;

namespace SwipeTransactionService.Tests.Integration;

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<CustomWebApplicationFactory> { }
