using Microsoft.AspNetCore.Mvc;
using CRM.Api.Data;
using CRM.Api.Models;

namespace CRM.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeadsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeadsController(AppDbContext context)
    {
        _context = context;
    }

    // GET /leads
    [HttpGet]
    public IActionResult Get()
    {
        var leads = _context.Leads.ToList();
        return Ok(leads);
    }

    // POST /leads
    [HttpPost]
    public IActionResult Create(Lead lead)
    {
        _context.Leads.Add(lead);
        _context.SaveChanges();
        return Ok(lead);
    }

    // DELETE /leads/1
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

