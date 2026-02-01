using Microsoft.AspNetCore.Mvc;
using CRM.Api.Data;
using CRM.Api.Models;

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

    // =========================================
    // GET: api/leads
    // Get all leads
    // =========================================
    [HttpGet]
    public IActionResult GetAll()
    {
        var leads = _context.Leads
                            .OrderByDescending(x => x.Id)
                            .ToList();

        return Ok(leads);
    }

    // =========================================
    // GET: api/leads/5
    // Get single lead
    // =========================================
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var lead = _context.Leads.Find(id);

        if (lead == null)
            return NotFound();

        return Ok(lead);
    }

    // =========================================
    // POST: api/leads
    // Create lead
    // =========================================
    [HttpPost]
    public IActionResult Create([FromBody] Lead lead)
    {
        if (lead == null)
            return BadRequest();

        lead.CreatedDate = DateTime.UtcNow;

        _context.Leads.Add(lead);
        _context.SaveChanges();

        return Ok(lead);
    }

    // =========================================
    // PUT: api/leads/5
    // Update lead
    // =========================================
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Lead updatedLead)
    {
        var lead = _context.Leads.Find(id);

        if (lead == null)
            return NotFound();

        lead.Name = updatedLead.Name;
        lead.Email = updatedLead.Email;
        lead.Phone = updatedLead.Phone;
        lead.Company = updatedLead.Company;
        lead.Source = updatedLead.Source;
        lead.Status = updatedLead.Status;

        _context.SaveChanges();

        return Ok(lead);
    }

    // =========================================
    // DELETE: api/leads/5
    // Delete lead
    // =========================================
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var lead = _context.Leads.Find(id);

        if (lead == null)
            return NotFound();

        _context.Leads.Remove(lead);
        _context.SaveChanges();

        return Ok();
    }
}
