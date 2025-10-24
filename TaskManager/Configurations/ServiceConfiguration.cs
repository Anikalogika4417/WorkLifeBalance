using Serilog;
using TaskManager.Services;

namespace TaskManager.Configurations;

public static class ServiceConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureLogging();
        builder.ConfigureSwagger();
        builder.ConfigureHttpClient();

        // CORS
        var corsOrigins = builder.Configuration.GetSection("Services:CorsOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(p => p.AddPolicy("default",
            b => b.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod()));

        builder.ConfigureOtherServices();

        return builder;
    }

    private static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
    }

    private static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
    private static void ConfigureHttpClient(this WebApplicationBuilder builder)
    {
        var httpClients = builder.Configuration.GetSection("Services:HttpClients")
            .Get<Dictionary<string, string>>() ?? new();

        foreach (var (clientName, baseUrl) in httpClients)
        {
            builder.Services.AddHttpClient(clientName, c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddPolicyHandler(PollyPolicies.GetRetryPolicy())
            .AddPolicyHandler(PollyPolicies.GetTimeoutPolicy());
        }
    }

    private static void ConfigureOtherServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICustomHttpHandler, CustomHttpHandler>();
    }
}
