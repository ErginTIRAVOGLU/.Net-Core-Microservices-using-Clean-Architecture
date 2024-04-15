using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Basket.Application;
using Basket.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Basket.Application.Commands;
using Basket.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddApiVersioning();
builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["CacheSettings:ConnectionString"], "Basket Redis Health", HealthStatus.Degraded);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "v1" });
});


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg => { 
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartHandler).GetTypeInfo().Assembly);

});

builder.Services.AddControllers();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDistributedCache, RedisCache>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["CacheSettings:ConnectionString"];
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health",
    new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.Run();
