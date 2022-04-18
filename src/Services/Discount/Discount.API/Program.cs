using Discount.API.Data;
using Discount.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DiscountDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<DiscountDbContext>>();
    logger.LogError("HELLOOOOOOO - " + builder.Configuration.GetConnectionString("DefaultConnection"));

    using (var context = scope.ServiceProvider.GetService<DiscountDbContext>())
    {
        context.Database.Migrate();
    }
}

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
