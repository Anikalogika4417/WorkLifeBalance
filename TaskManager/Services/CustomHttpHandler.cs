using System.Text;
using System.Text.Json;

namespace TaskManager.Services;

public sealed class CustomHttpHandler(IHttpClientFactory factory) : ICustomHttpHandler
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    { PropertyNameCaseInsensitive = true };

    public Task<T?> GetAsync<T>(string service, string endpoint,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default)
        => SendJsonAndRead<T>(HttpMethod.Get, service, endpoint, null, query, headers, ct);

    public Task<TOut?> PostJsonAsync<TIn, TOut>(string service, string endpoint, TIn? body,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default)
        => SendJsonAndRead<TOut>(HttpMethod.Post, service, endpoint, body, query, headers, ct);

    public Task<TOut?> PutJsonAsync<TIn, TOut>(string service, string endpoint, TIn? body,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default)
        => SendJsonAndRead<TOut>(HttpMethod.Put, service, endpoint, body, query, headers, ct);

    public async Task DeleteAsync(string service, string endpoint,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default)
    {
        using var resp = await SendAsync(HttpMethod.Delete, service, endpoint, null, query, headers, ct);
        await EnsureSuccess(resp);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string service, string endpoint,
        HttpContent? content = null, Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null, CancellationToken ct = default)
    {
        var client = factory.CreateClient(service); // BaseAddress уже есть
        var uri = BuildUri(client.BaseAddress!, endpoint, query);

        using var req = new HttpRequestMessage(method, uri) { Content = content };
        headers?.Invoke(req); // добавить Bearer/CorrelationId и пр.
        return await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
    }

    private async Task<T?> SendJsonAndRead<T>(HttpMethod method, string service, string endpoint, object? body,
        Dictionary<string, string?>? query, Action<HttpRequestMessage>? headers, CancellationToken ct)
    {
        using var content = body is null ? null :
            new StringContent(JsonSerializer.Serialize(body, JsonOpts), Encoding.UTF8, "application/json");

        using var resp = await SendAsync(method, service, endpoint, content, query, headers, ct);
        await EnsureSuccess(resp);

        if (resp.Content.Headers.ContentLength is 0) return default;
        return await resp.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    private static Uri BuildUri(Uri baseAddress, string endpoint, Dictionary<string, string?>? query)
    {
        var baseUrl = new Uri(baseAddress, endpoint.TrimStart('/')).ToString();
        if (query is null || query.Count == 0) return new Uri(baseUrl);
        var withQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(
            baseUrl, query.Where(kv => kv.Value is not null)
                          .ToDictionary(kv => kv.Key, kv => kv.Value!));
        return new Uri(withQuery);
    }

    private static async Task EnsureSuccess(HttpResponseMessage resp)
    {
        if (resp.IsSuccessStatusCode) return;
        string? body = null;
        try { body = await resp.Content.ReadAsStringAsync(); } catch { }
        throw new HttpRequestException($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {Short(body)}");
    }

    private static string Short(string? s) => string.IsNullOrEmpty(s) ? "" : s.Length <= 500 ? s : s[..500] + "…";
}
