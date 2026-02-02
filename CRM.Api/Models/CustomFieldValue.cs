namespace CRM.Api.Models;

public class CustomFieldValue
{
    public int Id { get; set; }

    public int RecordId { get; set; }   // LeadId
    public int FieldId { get; set; }

    public string? Value { get; set; }

    public CustomFieldDefinition Field { get; set; }
}
