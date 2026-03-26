using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace SwipeTransactionService.Tests.Integration;

/// <summary>
/// Integration tests for the HotChocolate GraphQL endpoint at /graphql.
/// Sends HTTP POST with application/json body.
/// NOTE: GetBatchSummary / GetDailyAvailed use Dapper (raw SQL) and are not
/// tested here as they require a real SQL Server connection.
/// </summary>
[Collection("Integration")]
public class GraphQLTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly HttpClient _anonClient;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public GraphQLTests(CustomWebApplicationFactory factory)
    {
        _client     = factory.CreateAuthenticatedClient();
        _anonClient = factory.CreateClient();
    }

    // ════════════════════════════════════════════════════════════════
    // Helpers
    // ════════════════════════════════════════════════════════════════

    private async Task<JsonElement> ExecuteAsync(string query, object? variables = null)
    {
        var body = new { query, variables };
        var response = await _client.PostAsJsonAsync("/graphql", body, _json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<JsonElement>(_json);
    }

    private async Task<JsonElement> ExecuteAnonAsync(string query, object? variables = null)
    {
        var body = new { query, variables };
        var response = await _anonClient.PostAsJsonAsync("/graphql", body, _json);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<JsonElement>(_json);
    }

    private static void AssertNoErrors(JsonElement doc)
    {
        if (doc.TryGetProperty("errors", out var errors))
            errors.GetArrayLength().Should().Be(0, $"GraphQL errors: {errors}");
    }

    // ════════════════════════════════════════════════════════════════
    // Introspection
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GraphQL_Introspection_ReturnsQueryRootType()
    {
        var doc = await ExecuteAsync("{ __typename }");

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("__typename").GetString()
           .Should().Be("Query");
    }

    [Fact]
    public async Task GraphQL_Endpoint_Returns200ForAnonymousIntrospection()
    {
        // HotChocolate allows introspection without auth by default
        var doc = await ExecuteAnonAsync("{ __typename }");

        doc.TryGetProperty("data", out _).Should().BeTrue();
    }

    // ════════════════════════════════════════════════════════════════
    // Query: getSwipesByEmployee
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_GetSwipesByEmployee_WhenNone_ReturnsEmptyArray()
    {
        var from = DateTime.UtcNow.AddDays(-1).ToString("o");
        var to   = DateTime.UtcNow.ToString("o");
        var doc  = await ExecuteAsync(
            """
            query($emp: String!, $from: DateTime!, $to: DateTime!) {
                swipesByEmployee(employeeNumber: $emp, from: $from, to: $to) {
                    employeeNumber
                    itemCode
                    updateStatus
                }
            }
            """,
            new { emp = "NOBODY", from, to });

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("swipesByEmployee")
           .GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task Query_GetSwipesByEmployee_AfterRecord_ReturnsSwipe()
    {
        // Seed a swipe via REST first
        var authClient = _client;
        var swipeTime  = DateTime.UtcNow.AddMinutes(-3);
        await authClient.PostAsJsonAsync("/api/swipetransactions", new
        {
            CompanyCode    = 1,
            EmployeeNumber = "GQL001",
            SwipeTime      = swipeTime,
            ItemCode       = 55,
            ItemQuantity   = 2,
            BatchNumber    = 500,
            SerialNumber   = 501,
            CanteenNumber  = "C",
            GateNumber     = "G05"
        }, _json);

        var from = swipeTime.AddMinutes(-1).ToString("o");
        var to   = swipeTime.AddMinutes(10).ToString("o");
        var doc  = await ExecuteAsync(
            """
            query($emp: String!, $from: DateTime!, $to: DateTime!) {
                swipesByEmployee(employeeNumber: $emp, from: $from, to: $to) {
                    employeeNumber
                    itemCode
                    updateStatus
                }
            }
            """,
            new { emp = "GQL001", from, to });

        AssertNoErrors(doc);
        var swipes = doc.GetProperty("data").GetProperty("swipesByEmployee");
        swipes.GetArrayLength().Should().Be(1);
        swipes[0].GetProperty("employeeNumber").GetString().Should().Be("GQL001");
        swipes[0].GetProperty("itemCode").GetInt64().Should().Be(55);
        swipes[0].GetProperty("updateStatus").GetString().Should().Be("P");
    }

    // ════════════════════════════════════════════════════════════════
    // Query: getTodayPunch
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Query_GetTodayPunch_WhenNone_ReturnsNull()
    {
        var doc = await ExecuteAsync(
            """
            query($id: Long!) {
                todayPunch(empSysId: $id) {
                    employeeSysId
                    timeIn
                    timeOut
                }
            }
            """,
            new { id = 888888L });

        AssertNoErrors(doc);
        doc.GetProperty("data").GetProperty("todayPunch").ValueKind
           .Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task Query_GetTodayPunch_AfterCheckIn_ReturnsDto()
    {
        // Seed a check-in via REST
        await _client.PostAsJsonAsync("/api/canteenpunch", new
        {
            EmployeeSysId = 7001L,
            CanteenUnit   = 1,
            PunchType     = "I",
            PunchTime     = DateTime.UtcNow
        }, _json);

        var doc = await ExecuteAsync(
            """
            query($id: Long!) {
                todayPunch(empSysId: $id) {
                    employeeSysId
                    timeIn
                }
            }
            """,
            new { id = 7001L });

        AssertNoErrors(doc);
        var punch = doc.GetProperty("data").GetProperty("todayPunch");
        punch.ValueKind.Should().NotBe(JsonValueKind.Null);
        punch.GetProperty("employeeSysId").GetInt64().Should().Be(7001);
        punch.GetProperty("timeIn").GetString().Should().NotBeNullOrWhiteSpace();
    }

    // ════════════════════════════════════════════════════════════════
    // Mutation: recordSwipeUpload
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Mutation_RecordSwipeUpload_ReturnsCreatedSwipe()
    {
        var swipeTime = DateTime.UtcNow.AddMinutes(-2).ToString("o");
        var doc = await ExecuteAsync(
            """
            mutation($input: RecordSwipeUploadCommandInput!) {
                recordSwipeUpload(input: $input) {
                    employeeNumber
                    itemCode
                    updateStatus
                    serialNumber
                }
            }
            """,
            new
            {
                input = new
                {
                    companyCode    = 1,
                    employeeNumber = "GQL_MUT01",
                    swipeTime,
                    itemCode       = 30,
                    itemQuantity   = 1,
                    batchNumber    = 300,
                    serialNumber   = 301,
                    canteenNumber  = "D",
                    gateNumber     = "G03"
                }
            });

        AssertNoErrors(doc);
        var result = doc.GetProperty("data").GetProperty("recordSwipeUpload");
        result.GetProperty("employeeNumber").GetString().Should().Be("GQL_MUT01");
        result.GetProperty("itemCode").GetInt64().Should().Be(30);
        result.GetProperty("updateStatus").GetString().Should().Be("P");
    }

    // ════════════════════════════════════════════════════════════════
    // Mutation: recordPunch
    // ════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Mutation_RecordPunch_CheckIn_ReturnsPunchDto()
    {
        var punchTime = DateTime.UtcNow.AddMinutes(-1).ToString("o");
        var doc = await ExecuteAsync(
            """
            mutation($input: RecordPunchCommandInput!) {
                recordPunch(input: $input) {
                    employeeSysId
                    canteenUnit
                    timeIn
                }
            }
            """,
            new
            {
                input = new
                {
                    employeeSysId = 8001L,
                    canteenUnit   = 1,
                    punchType     = "I",
                    punchTime
                }
            });

        AssertNoErrors(doc);
        var result = doc.GetProperty("data").GetProperty("recordPunch");
        result.GetProperty("employeeSysId").GetInt64().Should().Be(8001);
        result.GetProperty("timeIn").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Mutation_RecordPunch_CheckOut_AfterCheckIn_SetsTimeOut()
    {
        var checkInTime  = DateTime.UtcNow.Date.AddHours(10).ToString("o");
        var checkOutTime = DateTime.UtcNow.Date.AddHours(11).ToString("o");

        // Check-in
        await ExecuteAsync(
            """
            mutation($input: RecordPunchCommandInput!) {
                recordPunch(input: $input) { employeeSysId }
            }
            """,
            new { input = new { employeeSysId = 8002L, canteenUnit = 1, punchType = "I", punchTime = checkInTime } });

        // Check-out
        var doc = await ExecuteAsync(
            """
            mutation($input: RecordPunchCommandInput!) {
                recordPunch(input: $input) {
                    employeeSysId
                    timeIn
                    timeOut
                    workHours
                }
            }
            """,
            new { input = new { employeeSysId = 8002L, canteenUnit = 1, punchType = "O", punchTime = checkOutTime } });

        AssertNoErrors(doc);
        var result = doc.GetProperty("data").GetProperty("recordPunch");
        result.GetProperty("timeOut").GetString().Should().NotBeNullOrWhiteSpace();
        result.GetProperty("workHours").GetDecimal().Should().Be(1m);
    }
}
