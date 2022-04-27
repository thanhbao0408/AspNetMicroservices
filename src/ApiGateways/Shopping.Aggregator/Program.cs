using Common.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

var getRetryPolicy = () =>
{
    // In this case will wait for
    //  2 ^ 1 = 2 seconds then
    //  2 ^ 2 = 4 seconds then
    //  2 ^ 3 = 8 seconds then
    //  2 ^ 4 = 16 seconds then
    //  2 ^ 5 = 32 seconds

    return HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(
        retryCount: 5,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (exception, retryCount, context) =>
        {
            Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
        });
};

var getCircuitBreakerPolicy = () =>
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );
};

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddPolicyHandler(getRetryPolicy())
            .AddPolicyHandler(getCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
.AddHttpMessageHandler<LoggingDelegatingHandler>()
.AddPolicyHandler(getRetryPolicy())
.AddPolicyHandler(getCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
.AddHttpMessageHandler<LoggingDelegatingHandler>()
.AddPolicyHandler(getRetryPolicy())
.AddPolicyHandler(getCircuitBreakerPolicy());


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
});

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

