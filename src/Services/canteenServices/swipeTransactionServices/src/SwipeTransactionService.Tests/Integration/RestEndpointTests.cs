using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace SwipeTransactionService.Tests.Integration;

[Collection("Integration")]
public class RestEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public RestEndpointTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    // ════════════════════════════════════════════════════════════════
    // Auth
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Login_WithValidCredentials_Returns200WithToken()
    {
        var client  = _factory.CreateClient();
        var payload = JsonContent.Create(new { Username = "admin", Password = "P@ssw0rd!" });

        var response = await client.PostAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        body.GetProperty("expiresAt").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns401()
    {
        var client  = _factory.CreateClient();
        var payload = JsonContent.Create(new { Username = "admin", Password = "wrong" });

        var response = await client.PostAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithWrongUsername_Returns401()
    {
        var client  = _factory.CreateClient();
        var payload = JsonContent.Create(new { Username = "hacker", Password = "P@ssw0rd!" });

        var response = await client.PostAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ════════════════════════════════════════════════════════════════
    // Auth guard
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetPendingSwipes_WithoutToken_Returns401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/swipetransactions/pending");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPendingSwipes_WithInvalidToken_Returns401()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "not.a.valid.token");

        var response = await client.GetAsync("/api/swipetransactions/pending");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ════════════════════════════════════════════════════════════════
    // SwipeTransactions — CRUD
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetPendingSwipes_WhenEmpty_Returns200WithEmptyArray()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/swipetransactions/pending");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task RecordSwipe_WithValidPayload_Returns201WithSwipe()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CompanyCode    = 1,
            EmployeeNumber = "EMP001",
            SwipeTime      = DateTime.UtcNow.AddMinutes(-1),
            ItemCode       = 10,
            ItemQuantity   = 1,
            BatchNumber    = 100,
            SerialNumber   = 1,
            CanteenNumber  = "A",
            GateNumber     = "G01"
        });

        var response = await client.PostAsync("/api/swipetransactions", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeNumber").GetString().Should().Be("EMP001");
        body.GetProperty("updateStatus").GetString().Should().Be("P");
    }

    [Fact]
    public async Task GetSwipesByEmployee_AfterRecord_ReturnsSwipe()
    {
        var client = _factory.CreateAuthenticatedClient();
        var swipeTime = DateTime.UtcNow.AddMinutes(-5);
        await RecordSwipeAsync(client, "EMP100", swipeTime, serialNumber: 50);

        var from = swipeTime.AddMinutes(-1).ToString("o");
        var to   = swipeTime.AddMinutes(10).ToString("o");
        var response = await client.GetAsync($"/api/swipetransactions/EMP100?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().ContainSingle(e => e.GetProperty("employeeNumber").GetString() == "EMP100");
    }

    [Fact]
    public async Task GetSwipesByEmployee_WhenNoneExist_ReturnsEmptyArray()
    {
        var client = _factory.CreateAuthenticatedClient();
        var from = DateTime.UtcNow.AddDays(-1).ToString("o");
        var to   = DateTime.UtcNow.ToString("o");

        var response = await client.GetAsync($"/api/swipetransactions/UNKNOWN_EMP?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task RecordSwipe_WithEmptyEmployeeNumber_Returns400()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CompanyCode    = 1,
            EmployeeNumber = "",       // invalid
            SwipeTime      = DateTime.UtcNow.AddMinutes(-1),
            ItemCode       = 10,
            ItemQuantity   = 1,
            BatchNumber    = 100,
            SerialNumber   = 2,
            CanteenNumber  = "A",
            GateNumber     = "G01"
        });

        var response = await client.PostAsync("/api/swipetransactions", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ════════════════════════════════════════════════════════════════
    // CanteenPunch
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetTodayPunch_WhenNone_Returns404()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/canteenpunch/999999/today");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RecordPunch_CheckIn_Returns200WithPunchDto()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            EmployeeSysId = 1001,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = DateTime.UtcNow.AddMinutes(-2)
        });

        var response = await client.PostAsync("/api/canteenpunch", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeSysId").GetInt64().Should().Be(1001);
        body.GetProperty("timeIn").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task RecordPunch_CheckOut_AfterCheckIn_Returns200WithWorkHours()
    {
        var client    = _factory.CreateAuthenticatedClient();
        var checkInTime  = DateTime.UtcNow.Date.AddHours(8);

        // Check-in
        await client.PostAsync("/api/canteenpunch", JsonContent.Create(new
        {
            EmployeeSysId = 1002,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = checkInTime
        }));

        // Check-out 1 hour later
        var payload = JsonContent.Create(new
        {
            EmployeeSysId = 1002,
            CanteenUnit   = 1,
            PunchType     = "O",
            PunchTime     = checkInTime.AddHours(1)
        });
        var response = await client.PostAsync("/api/canteenpunch", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("timeOut").GetString().Should().NotBeNullOrWhiteSpace();
        body.GetProperty("workHours").GetDecimal().Should().Be(1m);
    }

    [Fact]
    public async Task GetTodayPunch_AfterCheckIn_Returns200()
    {
        var client = _factory.CreateAuthenticatedClient();
        await client.PostAsync("/api/canteenpunch", JsonContent.Create(new
        {
            EmployeeSysId = 2001,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = DateTime.UtcNow
        }));

        var response = await client.GetAsync("/api/canteenpunch/2001/today");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeSysId").GetInt64().Should().Be(2001);
    }

    [Fact]
    public async Task GetPunches_ByEmployee_ReturnsRangeResults()
    {
        var client = _factory.CreateAuthenticatedClient();
        var punchTime = DateTime.UtcNow.Date.AddHours(9);
        await client.PostAsync("/api/canteenpunch", JsonContent.Create(new
        {
            EmployeeSysId = 3001,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = punchTime
        }));

        var from = punchTime.AddDays(-1).ToString("o");
        var to   = punchTime.AddDays(1).ToString("o");
        var response = await client.GetAsync($"/api/canteenpunch/3001?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<JsonElement[]>(_json);
        items.Should().ContainSingle(e => e.GetProperty("employeeSysId").GetInt64() == 3001);
    }

    // ════════════════════════════════════════════════════════════════
    // Minimal API v2 — Swipes
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task MinimalApi_GetPendingSwipes_WithoutToken_Returns401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v2/swipes/pending");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task MinimalApi_CreateSwipe_Returns201()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            CompanyCode    = 1,
            EmployeeNumber = "EMP200",
            SwipeTime      = DateTime.UtcNow.AddMinutes(-1),
            ItemCode       = 20,
            ItemQuantity   = 1,
            BatchNumber    = 200,
            SerialNumber   = 200,
            CanteenNumber  = "B",
            GateNumber     = "G02"
        });

        var response = await client.PostAsync("/api/v2/swipes", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeNumber").GetString().Should().Be("EMP200");
    }

    [Fact]
    public async Task MinimalApi_GetPendingSwipes_ReturnsOk()
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/v2/swipes/pending");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MinimalApi_GetSwipesByRange_ReturnsOk()
    {
        var client = _factory.CreateAuthenticatedClient();
        var from = DateTime.UtcNow.AddDays(-1).ToString("o");
        var to   = DateTime.UtcNow.ToString("o");

        var response = await client.GetAsync($"/api/v2/swipes/EMP300/range?from={from}&to={to}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ════════════════════════════════════════════════════════════════
    // Minimal API v2 — Punches
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task MinimalApi_RecordPunch_Returns200()
    {
        var client  = _factory.CreateAuthenticatedClient();
        var payload = JsonContent.Create(new
        {
            EmployeeSysId = 4001,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = DateTime.UtcNow
        });

        var response = await client.PostAsync("/api/v2/punches", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(_json);
        body.GetProperty("employeeSysId").GetInt64().Should().Be(4001);
    }

    // ════════════════════════════════════════════════════════════════
    // Health checks
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Health_AlwaysReturnsHealthy()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HealthReady_ReturnsHealthy()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/ready");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ════════════════════════════════════════════════════════════════
    // Helpers
    // ════════════════════════════════════════════════════════════════

    private static async Task RecordSwipeAsync(HttpClient client, string employeeNumber,
        DateTime swipeTime, long serialNumber = 1)
    {
        var payload = JsonContent.Create(new
        {
            CompanyCode    = 1,
            EmployeeNumber = employeeNumber,
            SwipeTime      = swipeTime,
            ItemCode       = 10,
            ItemQuantity   = 1,
            BatchNumber    = 100,
            SerialNumber   = serialNumber,
            CanteenNumber  = "A",
            GateNumber     = "G01"
        });
        var response = await client.PostAsync("/api/swipetransactions", payload);
        response.EnsureSuccessStatusCode();
    }
}
