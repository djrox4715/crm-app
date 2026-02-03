namespace CRM.Api.Models;

public class ViewColumn
{
    public int Id { get; set; }
    
    public int ViewDefinitionId { get; set; }
    
    /// <summary>
    /// The field name (e.g., "Name", "Email", or custom field label)
    /// </summary>
    public required string FieldName { get; set; }
    
    /// <summary>
    /// Display label for the column
    /// </summary>
    public required string Label { get; set; }
    
    /// <summary>
    /// Is this column visible in the view?
    /// </summary>
    public bool IsVisible { get; set; } = true;
    
    /// <summary>
    /// Column width in pixels
    /// </summary>
    public int? Width { get; set; }
    
    /// <summary>
    /// Display order (0-based index)
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// Is this a custom field?
    /// </summary>
    public bool IsCustomField { get; set; }
    
    // Navigation property
    public ViewDefinition ViewDefinition { get; set; } = null!;
}