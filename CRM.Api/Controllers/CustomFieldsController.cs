using Microsoft.AspNetCore.Mvc;
using CRM.Api.Data;
using CRM.Api.Models;

namespace CRM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomFieldsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomFieldsController(AppDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // GET: api/customfields?module=Lead
    // Get fields for a module
    // =====================================================
    [HttpGet]
    public IActionResult GetByModule([FromQuery] string module)
    {
        if (string.IsNullOrEmpty(module))
            return BadRequest("Module is required");

        var fields = _context.CustomFieldDefinitions
                             .Where(f => f.Module == module)
                             .OrderBy(f => f.Id)
                             .ToList();

        return Ok(fields);
    }


    // =====================================================
    // POST: api/customfields
    // Create new custom field
    // =====================================================
    [HttpPost]
    public IActionResult Create([FromBody] CustomFieldDefinition field)
    {
        if (field == null)
            return BadRequest();

        if (string.IsNullOrEmpty(field.Module) ||
            string.IsNullOrEmpty(field.Name) ||
            string.IsNullOrEmpty(field.Type))
        {
            return BadRequest("Module, Name and Type are required");
        }

        // prevent duplicate names in same module
        var exists = _context.CustomFieldDefinitions.Any(f =>
            f.Module == field.Module && f.Name == field.Name);

        if (exists)
            return Conflict("Field already exists");

        _context.CustomFieldDefinitions.Add(field);
        _context.SaveChanges();

        return Ok(field);
    }


    // =====================================================
    // DELETE: api/customfields/5
    // Delete field + values
    // =====================================================
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var field = _context.CustomFieldDefinitions.Find(id);

        if (field == null)
            return NotFound();

        // delete related values also
        var values = _context.CustomFieldValues
                             .Where(v => v.FieldId == id);

        _context.CustomFieldValues.RemoveRange(values);

        _context.CustomFieldDefinitions.Remove(field);

        _context.SaveChanges();

        return Ok();
    }
    [HttpGet("test")]
public IActionResult Test()
{
    return Ok("Working");
}

}
