using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace ItemMasterService.Tests.Integration;

[Collection("Integration")]
public class RestEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public RestEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // ════════════════════════════════════════════════════════════════
    // Auth endpoint
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task PostToken_WithNonEmptyCredentials_Returns200WithToken()
    {
        var client  = _factory.CreateClient();
        var payload = JsonContent.Create(new { Username = "testuser", Password = "anypassword" });

        var response = await client.PostAsync("/api/auth/token", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>(_json);
        body!.Token.Should().NotBeNullOrWhiteSpace();
        body.Expires.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task PostToken_WithEmptyUsername_Returns401()
    {
        var client  = _factory.CreateClient();
        var payload = JsonContent.Create(new { Username = "", Password = "pass" });

        var response = await client.PostAsync("/api/auth/token", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ════════════════════════════════════════════════════════════════
    // Authentication guard
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetCanteenItems_WithoutToken_Returns401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/CanteenItemMaster/1001");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCanteenItems_WithInvalidToken_Returns401()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "invalid.token.here");

        var response = await client.GetAsync("/api/CanteenItemMaster/1001");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ════════════════════════════════════════════════════════════════
    // CanteenItemMaster CRUD
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetCanteenItems_WhenEmpty_Returns200WithEmptyArray()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/CanteenItemMaster/9001");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCanteenItem_WithValidPayload_Returns201WithCreatedItem()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode  = 2001,
            ItemCode         = 1,
            ItemDescription  = "Test Meal",
            ItemType         = "F",
            ItemReference    = "TSTMEAL",
            EnteredBy        = "integtest"
        });

        var response = await client.PostAsync("/api/CanteenItemMaster", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("canteenUnitCode").GetInt64().Should().Be(2001);
        body.GetProperty("itemCode").GetInt64().Should().Be(1);
        body.GetProperty("itemDescription").GetString().Should().Be("Test Meal");
    }

    [Fact]
    public async Task GetCanteenItemById_AfterCreate_Returns200()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 2002, 1, "Rice Meal");

        var response = await client.GetAsync("/api/CanteenItemMaster/2002/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("itemDescription").GetString().Should().Be("Rice Meal");
    }

    [Fact]
    public async Task GetCanteenItemById_WhenNotFound_Returns404()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/CanteenItemMaster/9999/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllCanteenItems_AfterCreate_ReturnsItem()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 2003, 1, "Chicken Soup");

        var response = await client.GetAsync("/api/CanteenItemMaster/2003");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().ContainSingle(e => e.GetProperty("itemDescription").GetString() == "Chicken Soup");
    }

    [Fact]
    public async Task CreateDuplicateItem_Returns409Conflict()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 2010, 1, "First Item");

        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 2010, ItemCode = 1, ItemDescription = "Duplicate",
            ItemType = "F", ItemReference = "DUP", EnteredBy = "tester"
        });
        var response = await client.PostAsync("/api/CanteenItemMaster", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateCanteenItem_WithValidPayload_Returns200WithUpdatedItem()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 2004, 1, "Original");

        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 2004, ItemCode = 1,
            ItemDescription = "Updated Desc", ItemType = "B", ItemReference = "UPREF"
        });
        var response = await client.PutAsync("/api/CanteenItemMaster/2004/1", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("itemDescription").GetString().Should().Be("Updated Desc");
        body.GetProperty("itemType").GetString().Should().Be("B");
    }

    [Fact]
    public async Task UpdateCanteenItem_WithMismatchedIds_Returns400()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 9999, ItemCode = 1,
            ItemDescription = "Mismatch", ItemType = "F", ItemReference = "X"
        });

        var response = await client.PutAsync("/api/CanteenItemMaster/1001/1", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteCanteenItem_AfterCreate_Returns204()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 2005, 1, "Delete Me");

        var response = await client.DeleteAsync("/api/CanteenItemMaster/2005/1");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCanteenItem_WhenNotFound_Returns404()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.DeleteAsync("/api/CanteenItemMaster/8888/8888");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ════════════════════════════════════════════════════════════════
    // CanteenItemPrice endpoints
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetActivePrice_WhenNoPriceSet_Returns404()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 3001, 1, "Price Test Item");

        var response = await client.GetAsync("/api/CanteenItemPrice/3001/1/active");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateItemPrice_Returns201WithPrice()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 3002, 1, "Priced Item");

        var payload = JsonContent.Create(new
        {
            CanteenUnitCode     = 3002,
            ItemCode            = 1,
            EmployeeContribution = 25,
            EmployerContribution = 50,
            EffectiveDate       = DateTime.UtcNow.Date,
            EnteredBy           = "integtest"
        });
        var response = await client.PostAsync("/api/CanteenItemPrice", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeContribution").GetDecimal().Should().Be(25m);
        body.GetProperty("closureDate").GetRawText().Should().Be("null");
    }

    [Fact]
    public async Task GetActivePrice_AfterCreate_Returns200()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 3003, 1, "Active Price Test");
        await CreatePriceAsync(client, 3003, 1, 30m, 60m);

        var response = await client.GetAsync("/api/CanteenItemPrice/3003/1/active");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeContribution").GetDecimal().Should().Be(30m);
    }

    [Fact]
    public async Task GetPriceHistory_AfterCreate_Returns200WithHistory()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 3004, 1, "History Test");
        await CreatePriceAsync(client, 3004, 1, 20m, 40m);

        var response = await client.GetAsync("/api/CanteenItemPrice/3004/1/history");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().ContainSingle();
    }

    [Fact]
    public async Task CloseItemPrice_AfterCreate_Returns204()
    {
        var client = _factory.CreateAuthenticatedClient();
        await CreateItemAsync(client, 3005, 1, "Close Price Test");
        await CreatePriceAsync(client, 3005, 1, 20m, 40m);

        var closureDate  = JsonContent.Create(DateTime.UtcNow.Date.AddDays(30));
        var response = await client.PatchAsync("/api/CanteenItemPrice/3005/1/close", closureDate);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // ════════════════════════════════════════════════════════════════
    // Minimal API (v2)
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task MinimalApi_GetAll_WithoutToken_Returns401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v2/canteen-items/1001");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task MinimalApi_GetAll_WhenEmpty_Returns200()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/v2/canteen-items/7001");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task MinimalApi_Create_Returns201()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 7002, ItemCode = 1, ItemDescription = "Minimal Item",
            ItemType = "F", ItemReference = "MINAPI", EnteredBy = "test"
        });

        var response = await client.PostAsync("/api/v2/canteen-items/", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task MinimalApi_GetById_AfterCreate_Returns200()
    {
        var client = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 7003, ItemCode = 1, ItemDescription = "Min Get Test",
            ItemType = "F", ItemReference = "MGTEST", EnteredBy = "test"
        });
        await client.PostAsync("/api/v2/canteen-items/", payload);

        var response = await client.GetAsync("/api/v2/canteen-items/7003/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MinimalApi_Delete_Returns204()
    {
        var client = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = 7004, ItemCode = 1, ItemDescription = "Min Del Test",
            ItemType = "F", ItemReference = "MDTEST", EnteredBy = "test"
        });
        await client.PostAsync("/api/v2/canteen-items/", payload);

        var response = await client.DeleteAsync("/api/v2/canteen-items/7004/1");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // ════════════════════════════════════════════════════════════════
    // Health Checks
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task HealthLive_AlwaysReturns200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/live");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HealthReady_ReturnsHealthyInTestEnvironment()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/ready");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ════════════════════════════════════════════════════════════════
    // Helpers
    // ════════════════════════════════════════════════════════════════

    private static async Task CreateItemAsync(HttpClient client, long unitCode, long itemCode, string description)
    {
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode = unitCode,
            ItemCode        = itemCode,
            ItemDescription = description,
            ItemType        = "F",
            ItemReference   = "REF",
            EnteredBy       = "test"
        });
        var response = await client.PostAsync("/api/CanteenItemMaster", payload);
        response.EnsureSuccessStatusCode();
    }

    private static async Task CreatePriceAsync(HttpClient client, long unitCode, long itemCode,
        decimal empContrib, decimal eplContrib)
    {
        var payload = JsonContent.Create(new
        {
            CanteenUnitCode      = unitCode,
            ItemCode             = itemCode,
            EmployeeContribution  = empContrib,
            EmployerContribution  = eplContrib,
            EffectiveDate        = DateTime.UtcNow.Date,
            EnteredBy            = "test"
        });
        var response = await client.PostAsync("/api/CanteenItemPrice", payload);
        response.EnsureSuccessStatusCode();
    }

    private record TokenResponse(string Token, DateTime Expires);
}
