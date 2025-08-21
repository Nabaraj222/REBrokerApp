using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using REBrokerApp.API;
using REBrokerApp.Business.Services;
using Microsoft.Extensions.Caching.Memory;
using REBrokerApp.Domain.Interfaces;
using REBrokerApp.Infrastructure.ApplicationConstants;
using REBrokerApp.Infrastructure.Repositories;
using REBrokerApp.Web.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(ApplicationConstants.DefaultConnection) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add memory cache
builder.Services.AddMemoryCache();

// Register application services
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyFeatureRepository, PropertyFeatureRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<APIKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
