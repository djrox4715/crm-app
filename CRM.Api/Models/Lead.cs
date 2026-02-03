namespace CRM.Api.Models;

public class Lead
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }

    // ✅ NOT stored in DB - only used for API responses
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public Dictionary<string, object>? CustomFields { get; set; } = new();
}