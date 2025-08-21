using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace REBrokerApp.Web.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Domain.Entities.Property> Properties { get; set; } 
    public DbSet<Domain.Entities.PropertyFeature> PropertyFeature { get; set; } 
    public DbSet<Domain.Entities.PropertyLocation> PropertyLocation { get; set; } 
    public DbSet<Domain.Entities.PropertyImage> PropetyImages { get; set; } 
    public DbSet<Domain.Entities.BrokerCommission> BrokerCommissions { get; set; } 
    public DbSet<Domain.Entities.BrokerCommisionSetup> BrokerCommisionSetups { get; set; } 
}
