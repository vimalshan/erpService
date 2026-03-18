using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TdsService.Integration.Tests;

/// <summary>
/// Integration tests for the Vendors REST API
/// (authentication, CRUD operations, validation).
/// </summary>
public sealed class VendorsApiTests(TdsApiFactory factory)
    : IClassFixture<TdsApiFactory>
{
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    // ── helpers ──────────────────────────────────────────────────────────────

    private async Task<string> GetJwtTokenAsync()
    {
        var client = factory.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/auth/token",
            new { Username = "admin", Password = "Admin@1234" });
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        var node = JsonNode.Parse(json)!;
        return node["accessToken"]!.GetValue<string>();
    }

    private async Task<HttpClient> AuthenticatedClientAsync()
    {
        var client = factory.CreateClient();
        var token = await GetJwtTokenAsync();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    // ── auth token endpoint ───────────────────────────────────────────────────

    [Fact]
    public async Task PostAuthToken_ValidCredentials_Returns200WithToken()
    {
        var client = factory.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/auth/token",
            new { Username = "admin", Password = "Admin@1234" });

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var json = await resp.Content.ReadAsStringAsync();
        var node = JsonNode.Parse(json)!;
        Assert.NotNull(node["accessToken"]);
        Assert.False(string.IsNullOrWhiteSpace(node["accessToken"]!.GetValue<string>()));
    }

    [Fact]
    public async Task PostAuthToken_WrongPassword_Returns401()
    {
        var client = factory.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/auth/token",
            new { Username = "admin", Password = "wrongpassword" });

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    // ── unauthenticated access ────────────────────────────────────────────────

    [Fact]
    public async Task GetVendors_WithoutToken_Returns401()
    {
        var client = factory.CreateClient();
        var resp = await client.GetAsync("/api/vendors");

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    // ── authenticated CRUD ────────────────────────────────────────────────────

    [Fact]
    public async Task GetVendors_WithToken_Returns200()
    {
        var client = await AuthenticatedClientAsync();

        var resp = await client.GetAsync("/api/vendors");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task CreateVendor_Valid_Returns201()
    {
        var client = await AuthenticatedClientAsync();
        var payload = new
        {
            VendorId = 101L,
            VendorName = "Test Vendor Pvt Ltd",
            EmailAddress = "vendor@example.com",
            PanNo = "ABCDE1234F"
        };

        var resp = await client.PostAsJsonAsync("/api/vendors", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
    }

    [Fact]
    public async Task CreateVendor_InvalidPan_Returns400()
    {
        var client = await AuthenticatedClientAsync();
        var payload = new
        {
            VendorName = "Bad Vendor",
            EmailAddress = "bad@example.com",
            PanNo = "INVALID"           // fails PAN regex
        };

        var resp = await client.PostAsJsonAsync("/api/vendors", payload);

        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact]
    public async Task CreateThenGetByPan_Returns200WithMatchingVendor()
    {
        var client = await AuthenticatedClientAsync();
        var pan = "XYZPQ9876K";
        var payload = new
        {
            VendorId = 202L,
            VendorName = "Pan Lookup Vendor",
            EmailAddress = "lookup@example.com",
            PanNo = pan
        };

        var create = await client.PostAsJsonAsync("/api/vendors", payload);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var get = await client.GetAsync($"/api/vendors/{pan}");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var json = await get.Content.ReadAsStringAsync();
        var node = JsonNode.Parse(json)!;
        Assert.Equal(pan, node["panNo"]!.GetValue<string>());
    }
}
