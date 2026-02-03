namespace CRM.Api.Models;

public class ViewFilter
{
    public int Id { get; set; }
    
    public int ViewDefinitionId { get; set; }
    
    /// <summary>
    /// Field to filter on
    /// </summary>
    public required string FieldName { get; set; }
    
    /// <summary>
    /// Filter operator: equals, contains, startsWith, greaterThan, lessThan, between, isEmpty, isNotEmpty
    /// </summary>
    public required string Operator { get; set; }
    
    /// <summary>
    /// Filter value (JSON for complex values like date ranges)
    /// </summary>
    public string? Value { get; set; }
    
    /// <summary>
    /// Is this a custom field filter?
    /// </summary>
    public bool IsCustomField { get; set; }
    
    // Navigation property
    public ViewDefinition ViewDefinition { get; set; } = null!;
}