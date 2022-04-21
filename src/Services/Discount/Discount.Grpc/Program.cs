using Discount.Grpc.Data;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Discount.Grpc;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Anchor));

Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<DiscountDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    using (var context = scope.ServiceProvider.GetService<DiscountDbContext>())
    {
        //Console.WriteLine(context.Database.GetConnectionString());
        //context.Database.Migrate();
    }
}

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
