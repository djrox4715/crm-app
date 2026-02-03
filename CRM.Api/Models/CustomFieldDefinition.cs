namespace CRM.Api.Models;

public class CustomFieldDefinition
{
    public int Id { get; set; }

    public string Module { get; set; }   // Lead, Opportunity
    public string Name { get; set; }     // budget
    public string Label { get; set; }    // Budget
    public string Type { get; set; }   // text, number, date, checkbox, dropdown
public string? OptionsJson { get; set; } // for dropdown only

    public bool Required { get; set; }
}
