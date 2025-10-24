using Polly;

namespace TaskManager.Configurations;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() 
        => Polly.Extensions.Http.HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => (int)r.StatusCode == 429)
            .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(200 * Math.Pow(2, i)));

    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(20));
}
