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

    // ================= GET =================
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var leads = await _context.Leads
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return Ok(leads);
    }

    // ================= POST =================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Lead lead)
    {
        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        return Ok(lead);
    }

    // ================= DELETE =================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _context.Leads.FindAsync(id);

        if (lead == null)
            return NotFound();

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
