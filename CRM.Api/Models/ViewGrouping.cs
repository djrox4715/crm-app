namespace CRM.Api.Models;

public class ViewGrouping
{
    public int Id { get; set; }
    
    public int ViewDefinitionId { get; set; }
    
    /// <summary>
    /// Field to group by
    /// </summary>
    public required string FieldName { get; set; }
    
    /// <summary>
    /// Grouping order for nested grouping (0-based)
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// Is this group expanded by default?
    /// </summary>
    public bool IsExpanded { get; set; } = true;
    
    /// <summary>
    /// Is this a custom field group?
    /// </summary>
    public bool IsCustomField { get; set; }
    
    // Navigation property
    public ViewDefinition ViewDefinition { get; set; } = null!;
}