using TaskManager;
using TaskManager.Configurations;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureServices();

var app = builder.Build();

app.ConfigureEndpoints();

app.MapGet("/", () => "Hello World!");

app.Run();
