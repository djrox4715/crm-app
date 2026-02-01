namespace CRM.Api.Models;

public class Lead
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }

    // NEW FIELDS
    public string Phone { get; set; }
    public string Company { get; set; }
    public string Source { get; set; }   // Website, Referral, Ads
    public string Status { get; set; }   // New, Contacted, Qualified

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
