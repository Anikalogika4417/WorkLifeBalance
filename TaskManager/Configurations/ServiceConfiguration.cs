using Serilog;

namespace TaskManager.Configurations;

public static class ServiceConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureLogging();
        builder.ConfigureSwagger();

        // CORS
        var corsOrigins = builder.Configuration.GetSection("Services:CorsOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(p => p.AddPolicy("default",
            b => b.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod()));

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
}
