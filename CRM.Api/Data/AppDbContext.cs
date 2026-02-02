using CRM.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lead> Leads { get; set; }

    // ⭐ ADD THESE
    public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }
    public DbSet<CustomFieldValue> CustomFieldValues { get; set; }
}
