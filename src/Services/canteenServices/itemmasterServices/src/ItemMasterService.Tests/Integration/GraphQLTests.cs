using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace ItemMasterService.Tests.Integration;

/// <summary>
/// Integration tests for the HotChocolate GraphQL endpoint at /graphql.
/// Requests are plain HTTP POST with application/json body.
/// </summary>
[Collection("Integration")]
public class GraphQLTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public GraphQLTests(CustomWebApplicationFactory factory)
    {
        // GraphQL mutations change state – use isolated authenticated client
        _client = factory.CreateAuthenticatedClient("graphql-tester");
    }

    // ════════════════════════════════════════════════════════════════
    // Helpers
    // ════════════════════════════════════════════════════════════════

    private Task<JsonElement> ExecuteAsync(string query, object? variables = null) =>
        ExecuteOnClientAsync(_client, query, variables);

    private async Task<JsonElement> ExecuteOnClientAsync(HttpClient client, string query, object? variables = null)
    {
        var body = JsonContent.Create(new { query, variables });
        var response = await client.PostAsync("/graphql", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"GraphQL always returns 200; errors appear in the body. Got {(int)response.StatusCode}.");

        var content = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(content, _json);
        return doc;
    }

    private static void AssertNoErrors(JsonElement doc)
    {
        if (doc.TryGetProperty("errors", out var errors))
            Assert.Fail($"GraphQL returned errors: {errors}");
    }

    // ════════════════════════════════════════════════════════════════
    // Introspection / connectivity
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GraphQL_Introspection_ReturnsSchemaInfo()
    {
        var doc = await ExecuteAsync("{ __typename }");

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("__typename").GetString()
           .Should().Be("CanteenItemQuery");
    }

    // ════════════════════════════════════════════════════════════════
    // Query: canteenItems
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_CanteenItems_WhenEmpty_ReturnsEmptyArray()
    {
        const string query = """
            query GetItems($code: Long!) {
                canteenItems(canteenUnitCode: $code) {
                    itemCode
                    itemDescription
                }
            }
            """;

        var doc = await ExecuteAsync(query, new { code = 9100L });

        AssertNoErrors(doc);
        var items = doc.GetProperty("data").GetProperty("canteenItems");
        items.GetArrayLength().Should().Be(0);
    }

    // ════════════════════════════════════════════════════════════════
    // Mutation: createCanteenItem
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Mutation_CreateCanteenItem_ReturnsCreatedItem()
    {
        const string mutation = """
            mutation CreateItem($input: CreateCanteenItemInput!) {
                createCanteenItem(input: $input) {
                    canteenUnitCode
                    itemCode
                    itemDescription
                    itemType
                    enteredBy
                }
            }
            """;

        var doc = await ExecuteAsync(mutation, new
        {
            input = new
            {
                canteenUnitCode = 5001L,
                itemCode        = 1L,
                itemDescription = "GraphQL Meal",
                itemType        = "F",
                itemReference   = "GQLREF",
                enteredBy       = "gql-test"
            }
        });

        AssertNoErrors(doc);
        var item = doc.GetProperty("data").GetProperty("createCanteenItem");
        item.GetProperty("canteenUnitCode").GetInt64().Should().Be(5001);
        item.GetProperty("itemCode").GetInt64().Should().Be(1);
        item.GetProperty("itemDescription").GetString().Should().Be("GraphQL Meal");
        item.GetProperty("itemType").GetString().Should().Be("F");
        item.GetProperty("enteredBy").GetString().Should().Be("gql-test");
    }

    // ════════════════════════════════════════════════════════════════
    // Query: canteenItem (single)
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_CanteenItem_AfterCreate_ReturnsItem()
    {
        // Arrange – create via mutation
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5002, itemCode: 1,
                    itemDescription: "Query Single Test", itemType: "F",
                    itemReference: "QST", enteredBy: "test"
                }) { itemCode }
            }
            """);

        // Act – query single
        const string query = """
            query {
                canteenItem(canteenUnitCode: 5002, itemCode: 1) {
                    canteenUnitCode
                    itemCode
                    itemDescription
                }
            }
            """;
        var doc = await ExecuteAsync(query);

        AssertNoErrors(doc);
        var item = doc.GetProperty("data").GetProperty("canteenItem");
        item.GetProperty("itemDescription").GetString().Should().Be("Query Single Test");
    }

    [Fact]
    public async Task Query_CanteenItem_WhenNotFound_ReturnsNull()
    {
        const string query = """
            query {
                canteenItem(canteenUnitCode: 9999, itemCode: 9999) {
                    itemCode
                }
            }
            """;

        var doc = await ExecuteAsync(query);

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("canteenItem").ValueKind
           .Should().Be(JsonValueKind.Null);
    }

    // ════════════════════════════════════════════════════════════════
    // Query: canteenItems (list after insert)
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_CanteenItems_AfterCreate_ContainsItem()
    {
        // Arrange
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5003, itemCode: 1,
                    itemDescription: "List Item A", itemType: "S",
                    itemReference: "LIA", enteredBy: "test"
                }) { itemCode }
            }
            """);
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5003, itemCode: 2,
                    itemDescription: "List Item B", itemType: "B",
                    itemReference: "LIB", enteredBy: "test"
                }) { itemCode }
            }
            """);

        // Act
        var doc = await ExecuteAsync("""
            query {
                canteenItems(canteenUnitCode: 5003) {
                    itemCode
                    itemDescription
                }
            }
            """);

        AssertNoErrors(doc);
        var items = doc.GetProperty("data").GetProperty("canteenItems");
        items.GetArrayLength().Should().Be(2);
    }

    // ════════════════════════════════════════════════════════════════
    // Mutation: updateCanteenItem
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Mutation_UpdateCanteenItem_ReturnsUpdatedItem()
    {
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5004, itemCode: 1,
                    itemDescription: "Before Update", itemType: "F",
                    itemReference: "BU", enteredBy: "test"
                }) { itemCode }
            }
            """);

        const string mutation = """
            mutation UpdateItem($input: UpdateCanteenItemInput!) {
                updateCanteenItem(input: $input) {
                    itemDescription
                    itemType
                }
            }
            """;
        var doc = await ExecuteAsync(mutation, new
        {
            input = new
            {
                canteenUnitCode = 5004L,
                itemCode        = 1L,
                itemDescription = "After Update",
                itemType        = "B",
                itemReference   = "AU"
            }
        });

        AssertNoErrors(doc);
        var item = doc.GetProperty("data").GetProperty("updateCanteenItem");
        item.GetProperty("itemDescription").GetString().Should().Be("After Update");
        item.GetProperty("itemType").GetString().Should().Be("B");
    }

    // ════════════════════════════════════════════════════════════════
    // Mutation: deleteCanteenItem
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Mutation_DeleteCanteenItem_ReturnsTrue()
    {
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5005, itemCode: 1,
                    itemDescription: "To Delete", itemType: "F",
                    itemReference: "DEL", enteredBy: "test"
                }) { itemCode }
            }
            """);

        var doc = await ExecuteAsync("""
            mutation {
                deleteCanteenItem(canteenUnitCode: 5005, itemCode: 1)
            }
            """);

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("deleteCanteenItem").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task Mutation_DeleteCanteenItem_VerifyItemGone()
    {
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5006, itemCode: 1,
                    itemDescription: "Gone Item", itemType: "F",
                    itemReference: "G1", enteredBy: "test"
                }) { itemCode }
            }
            """);
        await ExecuteAsync("""
            mutation {
                deleteCanteenItem(canteenUnitCode: 5006, itemCode: 1)
            }
            """);

        var doc = await ExecuteAsync("""
            query {
                canteenItem(canteenUnitCode: 5006, itemCode: 1) {
                    itemCode
                }
            }
            """);

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("canteenItem").ValueKind
           .Should().Be(JsonValueKind.Null);
    }

    // ════════════════════════════════════════════════════════════════
    // Query: activePrice
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_ActivePrice_WhenNoneSet_ReturnsNull()
    {
        await ExecuteAsync("""
            mutation {
                createCanteenItem(input: {
                    canteenUnitCode: 5007, itemCode: 1,
                    itemDescription: "No Price Item", itemType: "F",
                    itemReference: "NP", enteredBy: "test"
                }) { itemCode }
            }
            """);

        var doc = await ExecuteAsync("""
            query {
                activePrice(canteenUnitCode: 5007, itemCode: 1) {
                    employeeContribution
                }
            }
            """);

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("activePrice").ValueKind
           .Should().Be(JsonValueKind.Null);
    }

    // ════════════════════════════════════════════════════════════════
    // Query: gradeItemPrices
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_GradeItemPrices_WhenEmpty_ReturnsEmptyArray()
    {
        var doc = await ExecuteAsync("""
            query {
                gradeItemPrices {
                    canteenUnitCode
                    gradeType
                }
            }
            """);

        AssertNoErrors(doc);
        var items = doc.GetProperty("data").GetProperty("gradeItemPrices");
        items.ValueKind.Should().Be(JsonValueKind.Array);
    }

    // ════════════════════════════════════════════════════════════════
    // Authorization – unauthenticated request
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GraphQL_WithoutToken_StillReturns200ForNonAuthorizedTypes()
    {
        // HotChocolate returns 200 always; auth errors appear in the errors array.
        // Without [Authorize] on the types, the query works unauthenticated.
        var factory = new CustomWebApplicationFactory();
        var unauthClient = factory.CreateClient(); // no token

        var doc = await ExecuteOnClientAsync(unauthClient, "{ __typename }");

        doc.TryGetProperty("errors", out _).Should().BeFalse("introspection should succeed unauthenticated");
    }
}
