using REBrokerApp.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using REBrokerApp.Infrastructure.ApplicationConstants;
using REBrokerApp.Business.Services;
using REBrokerApp.Infrastructure.Repositories;
using REBrokerApp.Domain.Interfaces;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(ApplicationConstants.DefaultConnection) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString(ApplicationConstants.DefaultConnection), options =>
{
    options.CommandTimeout(600); // Set command timeout to 60 seconds (or adjust as needed)
    options.MigrationsAssembly("REBrokerApp.Infrastructure");
}));

// Register application services
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyFeatureRepository, PropertyFeatureRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
