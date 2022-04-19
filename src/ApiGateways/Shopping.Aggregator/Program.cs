using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]));
//.AddHttpMessageHandler<LoggingDelegatingHandler>()
//.AddPolicyHandler(GetRetryPolicy())
//.AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]));
//.AddHttpMessageHandler<LoggingDelegatingHandler>()
//.AddPolicyHandler(GetRetryPolicy())
//.AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]));
//.AddHttpMessageHandler<LoggingDelegatingHandler>()
//.AddPolicyHandler(GetRetryPolicy())
//.AddPolicyHandler(GetCircuitBreakerPolicy());


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
});

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
