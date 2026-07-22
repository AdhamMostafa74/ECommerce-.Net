using ECommerce.API;
using ECommerce.API.Endpoints;
using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateAsyncScope();
    var dbSeed = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    await dbSeed.SeedAll();
    await dbContext.Database.MigrateAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapProductEndpoints();
app.MapTypeEndpoints();
app.MapBrandEndpoints();
app.Run();
