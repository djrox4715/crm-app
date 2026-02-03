using CRM.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Leads Module
    public DbSet<Lead> Leads { get; set; }

    // Custom Fields System
    public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }
    public DbSet<CustomFieldValue> CustomFieldValues { get; set; }
    
    // View System
    public DbSet<ViewDefinition> ViewDefinitions { get; set; }
    public DbSet<ViewColumn> ViewColumns { get; set; }
    public DbSet<ViewFilter> ViewFilters { get; set; }
    public DbSet<ViewSorting> ViewSorting { get; set; }
    public DbSet<ViewGrouping> ViewGrouping { get; set; }
}