namespace TaskManager.Services;

public interface ICustomHttpHandler
{
    Task<T?> GetAsync<T>(string service, string endpoint,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default);

    Task<TOut?> PostJsonAsync<TIn, TOut>(string service, string endpoint, TIn? body,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default);

    Task<TOut?> PutJsonAsync<TIn, TOut>(string service, string endpoint, TIn? body,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default);

    Task DeleteAsync(string service, string endpoint,
        Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null,
        CancellationToken ct = default);

    Task<HttpResponseMessage> SendAsync(HttpMethod method, string service, string endpoint,
        HttpContent? content = null, Dictionary<string, string?>? query = null,
        Action<HttpRequestMessage>? headers = null, CancellationToken ct = default);
}
