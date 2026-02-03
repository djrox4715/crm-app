using CRM.Api.Data;
using CRM.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeadsController(AppDbContext context)
    {
        _context = context;
    }

    // ================= GET ALL =================
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var leads = await _context.Leads.ToListAsync();

        // Load custom fields for each lead
        foreach (var lead in leads)
        {
            await LoadCustomFields(lead);
        }

        return Ok(leads);
    }

    // ================= GET ONE =================
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        // Load custom fields
        await LoadCustomFields(lead);

        return Ok(lead);
    }

    // ================= CREATE =================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeadCreateDto dto)
    {
        // Create the lead with static fields only
        var lead = new Lead
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Company = dto.Company,
            Source = dto.Source,
            Status = dto.Status
        };

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        // Save custom fields to CustomFieldValues table
        if (dto.CustomFields != null)
        {
            await SaveCustomFields(lead.Id, dto.CustomFields);
        }

        // Load custom fields back for response
        await LoadCustomFields(lead);

        return Ok(lead);
    }

    // ================= UPDATE =================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeadCreateDto dto)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        // Update static fields
        lead.Name = dto.Name;
        lead.Email = dto.Email;
        lead.Phone = dto.Phone;
        lead.Company = dto.Company;
        lead.Source = dto.Source;
        lead.Status = dto.Status;

        await _context.SaveChangesAsync();

        // Delete existing custom field values
        var existingValues = await _context.CustomFieldValues
            .Where(v => v.RecordId == id)
            .ToListAsync();
        _context.CustomFieldValues.RemoveRange(existingValues);
        await _context.SaveChangesAsync();

        // Save new custom field values
        if (dto.CustomFields != null)
        {
            await SaveCustomFields(lead.Id, dto.CustomFields);
        }

        // Load custom fields back for response
        await LoadCustomFields(lead);

        return Ok(lead);
    }

    // ================= DELETE =================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        // Delete custom field values first
        var customFieldValues = await _context.CustomFieldValues
            .Where(v => v.RecordId == id)
            .ToListAsync();
        _context.CustomFieldValues.RemoveRange(customFieldValues);

        // Delete the lead
        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ================= HELPER METHODS =================

    /// <summary>
    /// Loads custom field values from CustomFieldValues table and populates Lead.CustomFields
    /// </summary>
    private async Task LoadCustomFields(Lead lead)
    {
        var customFieldValues = await _context.CustomFieldValues
            .Include(v => v.Field)
            .Where(v => v.RecordId == lead.Id)
            .ToListAsync();

        lead.CustomFields = new Dictionary<string, object>();

        foreach (var cfv in customFieldValues)
        {
            lead.CustomFields[cfv.Field.Label] = cfv.Value ?? "";
        }
    }

    /// <summary>
    /// Saves custom fields to CustomFieldValues table
    /// </summary>
    private async Task SaveCustomFields(int leadId, Dictionary<string, object> customFields)
    {
        foreach (var kvp in customFields)
        {
            var fieldLabel = kvp.Key;
            var fieldValue = kvp.Value?.ToString();

            // Find the custom field definition
            var fieldDef = await _context.CustomFieldDefinitions
                .FirstOrDefaultAsync(f => f.Label == fieldLabel && f.Module == "Lead");

            if (fieldDef == null) continue;

            // Create custom field value
            var cfv = new CustomFieldValue
            {
                RecordId = leadId,
                FieldId = fieldDef.Id,
                Value = fieldValue
            };

            _context.CustomFieldValues.Add(cfv);
        }

        await _context.SaveChangesAsync();
    }
}

// ================= DTO =================
public class LeadCreateDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}