namespace CRM.Api.Models;

public class ViewSorting
{
    public int Id { get; set; }
    
    public int ViewDefinitionId { get; set; }
    
    /// <summary>
    /// Field to sort by
    /// </summary>
    public required string FieldName { get; set; }
    
    /// <summary>
    /// Sort direction: asc or desc
    /// </summary>
    public required string Direction { get; set; }
    
    /// <summary>
    /// Sort order for multi-column sorting (0-based)
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// Is this a custom field sort?
    /// </summary>
    public bool IsCustomField { get; set; }
    
    // Navigation property
    public ViewDefinition ViewDefinition { get; set; } = null!;
}