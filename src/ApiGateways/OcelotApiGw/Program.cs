using Common.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json");
});

builder.Services.AddOcelot()
    .AddCacheManager(config =>
    {
        config.WithDictionaryHandle();
    });

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello World!");
});

app.UseOcelot();

app.Run();
