namespace CRM.Api.Models;

public class ViewDefinition
{
    public int Id { get; set; }
    
    /// <summary>
    /// Module this view belongs to (Lead, Opportunity, Customer, etc.)
    /// </summary>
    public required string Module { get; set; }
    
    /// <summary>
    /// User-friendly name for the view
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Is this the default view for the module?
    /// </summary>
    public bool IsDefault { get; set; }
    
    /// <summary>
    /// For future multi-tenant support
    /// </summary>
    public int? UserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<ViewColumn> Columns { get; set; } = new List<ViewColumn>();
    public ICollection<ViewFilter> Filters { get; set; } = new List<ViewFilter>();
    public ICollection<ViewSorting> Sorting { get; set; } = new List<ViewSorting>();
    public ICollection<ViewGrouping> Grouping { get; set; } = new List<ViewGrouping>();
}