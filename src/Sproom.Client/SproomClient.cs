using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sproom.Client.Models;

namespace Sproom.Client;

public class SproomClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public SproomClient(SproomClientOptions options)
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri(options.BaseUrl.TrimEnd('/'))
        };
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", options.ApiToken);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
    }

    public SproomClient(HttpClient httpClient)
    {
        _http = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
    }

    // ─── Documents ───────────────────────────────────────────────────

    public async Task<List<ApiDocument>> GetDocumentsAsync(int? skip = null, string? filter = null, CancellationToken ct = default)
    {
        var query = new List<string>();
        if (skip.HasValue) query.Add($"$skip={skip.Value}");
        if (!string.IsNullOrEmpty(filter)) query.Add($"$filter={Uri.EscapeDataString(filter)}");

        var url = "/api/documents";
        if (query.Count > 0) url += "?" + string.Join("&", query);

        return await GetAsync<List<ApiDocument>>(url, ct);
    }

    public async Task<Guid> SendDocumentAsync(byte[] documentContent, string? requestId = null, CancellationToken ct = default)
    {
        using var content = new ByteArrayContent(documentContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/documents") { Content = content };
        if (!string.IsNullOrEmpty(requestId))
            request.Headers.Add("X-Request-Id", requestId);

        using var response = await _http.SendAsync(request, ct);
        await EnsureSuccessAsync(response, ct);

        if (response.Headers.TryGetValues("X-Sproom-DocumentId", out var values))
        {
            return Guid.Parse(values.First());
        }

        throw new SproomApiException((int)response.StatusCode, null, "Missing X-Sproom-DocumentId header in response");
    }

    public async Task<byte[]> GetDocumentAsync(Guid documentId, ApiDocumentFormat format, CancellationToken ct = default)
    {
        var formatStr = format.ToString().ToLowerInvariant();
        var response = await _http.GetAsync($"/api/documents/{documentId}/{formatStr}", ct);
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task UpdateDocumentFieldsAsync(Guid documentId, List<EnrichField> fields, CancellationToken ct = default)
    {
        await PatchAsync($"/api/documents/{documentId}", fields, ct);
    }

    public async Task<List<DocumentStateEntry>> GetDocumentStateAsync(Guid documentId, CancellationToken ct = default)
    {
        return await GetAsync<List<DocumentStateEntry>>($"/api/documents/{documentId}/state", ct);
    }

    public async Task SetDocumentStateAsync(Guid documentId, StateChangeRequest stateChange, CancellationToken ct = default)
    {
        await PostAsync<object>($"/api/documents/{documentId}/state", stateChange, ct);
    }

    public async Task<List<ResponseStatus>> GetDocumentResponsesAsync(Guid documentId, CancellationToken ct = default)
    {
        return await GetAsync<List<ResponseStatus>>($"/api/documents/{documentId}/responses", ct);
    }

    public async Task<SetStatusResult> SetDocumentResponseAsync(Guid documentId, ResponseStateChangeRequest request, CancellationToken ct = default)
    {
        return await PostAsync<SetStatusResult>($"/api/documents/{documentId}/responses", request, ct);
    }

    public async Task<List<ApiDocument>> GetDocumentReferencesAsync(Guid documentId, CancellationToken ct = default)
    {
        return await GetAsync<List<ApiDocument>>($"/api/documents/{documentId}/references", ct);
    }

    // ─── Webhooks ────────────────────────────────────────────────────

    public async Task<List<WebhookDto>> GetWebhooksAsync(CancellationToken ct = default)
    {
        return await GetAsync<List<WebhookDto>>("/api/webhooks", ct);
    }

    public async Task<WebhookDto> CreateWebhookAsync(WebhookRequest request, CancellationToken ct = default)
    {
        return await PostAsync<WebhookDto>("/api/webhooks", request, ct);
    }

    public async Task<WebhookDto> GetWebhookAsync(Guid webhookId, CancellationToken ct = default)
    {
        return await GetAsync<WebhookDto>($"/api/webhooks/{webhookId}", ct);
    }

    public async Task<WebhookDto> UpdateWebhookAsync(Guid webhookId, WebhookRequest request, CancellationToken ct = default)
    {
        return await PutAsync<WebhookDto>($"/api/webhooks/{webhookId}", request, ct);
    }

    public async Task DeleteWebhookAsync(Guid webhookId, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/webhooks/{webhookId}", ct);
    }

    public async Task TestWebhookAsync(Guid webhookId, CancellationToken ct = default)
    {
        await GetRawAsync($"/api/webhooks/{webhookId}/test", ct);
    }

    public async Task<GetKeyResponse> GetWebhookPublicKeyAsync(CancellationToken ct = default)
    {
        return await GetAsync<GetKeyResponse>("/api/webhooks/key", ct);
    }

    // ─── Reports ─────────────────────────────────────────────────────

    public async Task<Guid> CreatePurchaseReportAsync(ReportRequest request, CancellationToken ct = default)
    {
        return await CreateReportAsync("/api/reports/purchases", request, ct);
    }

    public async Task<Guid> CreateSalesReportAsync(ReportRequest request, CancellationToken ct = default)
    {
        return await CreateReportAsync("/api/reports/sales", request, ct);
    }

    public async Task<byte[]> GetReportStatusAsync(Guid reportId, CancellationToken ct = default)
    {
        var response = await _http.GetAsync($"/api/reports/{reportId}/status", ct);
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task<byte[]> GetReportAsync(Guid reportId, CancellationToken ct = default)
    {
        var response = await _http.GetAsync($"/api/reports/{reportId}", ct);
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task DeleteReportAsync(Guid reportId, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/reports/{reportId}", ct);
    }

    // ─── Registrations ───────────────────────────────────────────────

    public async Task<List<RegistrationModel>> GetRegistrationsAsync(CancellationToken ct = default)
    {
        return await GetAsync<List<RegistrationModel>>("/api/registrations", ct);
    }

    public async Task<RegistrationModel> GetRegistrationAsync(Guid networkId, CancellationToken ct = default)
    {
        return await GetAsync<RegistrationModel>($"/api/registrations/{networkId}", ct);
    }

    public async Task DeleteRegistrationAsync(Guid networkId, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/registrations/{networkId}", ct);
    }

    public async Task<RegistrationResult> RegisterNemHandelAsync(NemHandelRegistrationRequest request, CancellationToken ct = default)
    {
        return await PostAsync<RegistrationResult>("/api/registrations/nemhandel", request, ct);
    }

    public async Task<RegistrationResult> RegisterPeppolAsync(PeppolRegistrationRequest request, CancellationToken ct = default)
    {
        return await PostAsync<RegistrationResult>("/api/registrations/peppol", request, ct);
    }

    // ─── PEPPOL Verifications ────────────────────────────────────────

    public async Task<InitiateVerificationResponse> InitiateVerificationAsync(InitiateVerificationRequest request, CancellationToken ct = default)
    {
        return await PostAsync<InitiateVerificationResponse>("/api/peppol-participant-verifications", request, ct);
    }

    public async Task<List<VerificationDto>> GetVerificationsAsync(CancellationToken ct = default)
    {
        return await GetAsync<List<VerificationDto>>("/api/peppol-participant-verifications", ct);
    }

    public async Task<VerificationDto> GetVerificationAsync(Guid id, CancellationToken ct = default)
    {
        return await GetAsync<VerificationDto>($"/api/peppol-participant-verifications/{id}", ct);
    }

    public async Task DeleteVerificationAsync(Guid id, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/peppol-participant-verifications/{id}", ct);
    }

    public async Task<byte[]> GetVerificationDocumentAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.GetAsync($"/api/peppol-participant-verifications/{id}/document", ct);
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task<byte[]> GetVerificationDocumentPreviewAsync(CancellationToken ct = default)
    {
        var response = await _http.GetAsync("/api/peppol-participant-verifications/document-preview", ct);
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    // ─── Recipients ──────────────────────────────────────────────────

    public async Task<LookupResult> GetRecipientAsync(string orgId, CancellationToken ct = default)
    {
        return await GetAsync<LookupResult>($"/api/recipients/{Uri.EscapeDataString(orgId)}", ct);
    }

    public async Task<List<LookupResult>> BulkLookupRecipientsAsync(List<string> identifiers, CancellationToken ct = default)
    {
        return await PostAsync<List<LookupResult>>("/api/recipients/bulk", identifiers, ct);
    }

    // ─── Subscriptions ───────────────────────────────────────────────

    public async Task<CreateSubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken ct = default)
    {
        return await PostAsync<CreateSubscriptionResponse>("/api/subscriptions", request, ct);
    }

    public async Task<List<GetSubscriptionResponse>> GetSubscriptionsAsync(CancellationToken ct = default)
    {
        return await GetAsync<List<GetSubscriptionResponse>>("/api/subscriptions", ct);
    }

    public async Task DeleteSubscriptionAsync(string serviceType, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/subscriptions/{Uri.EscapeDataString(serviceType)}", ct);
    }

    public async Task DeleteChildSubscriptionAsync(string serviceType, Guid companyId, CancellationToken ct = default)
    {
        await DeleteAsync($"/api/subscriptions/{Uri.EscapeDataString(serviceType)}/{companyId}", ct);
    }

    // ─── Health ──────────────────────────────────────────────────────

    public async Task<bool> IsHealthyAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _http.GetAsync("/api/health", ct);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // ─── Internal helpers ────────────────────────────────────────────

    private async Task<T> GetAsync<T>(string url, CancellationToken ct)
    {
        var response = await _http.GetAsync(url, ct);
        await EnsureSuccessAsync(response, ct);
        return (await response.Content.ReadFromJsonAsync<T>(_jsonOptions, ct))!;
    }

    private async Task<HttpResponseMessage> GetRawAsync(string url, CancellationToken ct)
    {
        var response = await _http.GetAsync(url, ct);
        await EnsureSuccessAsync(response, ct);
        return response;
    }

    private async Task<T> PostAsync<T>(string url, object body, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(body, _jsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync(url, content, ct);
        await EnsureSuccessAsync(response, ct);

        var responseBody = await response.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrEmpty(responseBody))
            return default!;

        return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions)!;
    }

    private async Task<T> PutAsync<T>(string url, object body, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(body, _jsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PutAsync(url, content, ct);
        await EnsureSuccessAsync(response, ct);
        return (await response.Content.ReadFromJsonAsync<T>(_jsonOptions, ct))!;
    }

    private async Task PatchAsync(string url, object body, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(body, _jsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
        var response = await _http.SendAsync(request, ct);
        await EnsureSuccessAsync(response, ct);
    }

    private async Task DeleteAsync(string url, CancellationToken ct)
    {
        var response = await _http.DeleteAsync(url, ct);
        await EnsureSuccessAsync(response, ct);
    }

    private async Task<Guid> CreateReportAsync(string url, ReportRequest request, CancellationToken ct)
    {
        var queryParams = $"?format={Uri.EscapeDataString(request.Format)}" +
                          $"&startDateUtc={request.StartDateUtc:O}" +
                          $"&endDateUtc={request.EndDateUtc:O}";

        var response = await _http.PostAsync(url + queryParams, null, ct);
        await EnsureSuccessAsync(response, ct);

        if (response.Headers.Location is { } location)
        {
            var segments = location.AbsolutePath.Split('/');
            return Guid.Parse(segments.Last());
        }

        throw new SproomApiException((int)response.StatusCode, null, "Missing Location header in report response");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? body = null;
        try { body = await response.Content.ReadAsStringAsync(ct); } catch { }

        ErrorResponse? error = null;
        if (!string.IsNullOrEmpty(body))
        {
            try
            {
                error = JsonSerializer.Deserialize<ErrorResponse>(body, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch { }
        }

        throw new SproomApiException(
            (int)response.StatusCode,
            error?.ErrorCode,
            error?.Message ?? body
        );
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}
