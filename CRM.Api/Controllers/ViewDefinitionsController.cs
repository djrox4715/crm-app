using CRM.Api.Data;
using CRM.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewDefinitionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ViewDefinitionsController(AppDbContext context)
    {
        _context = context;
    }

    // ================= GET ALL VIEWS FOR A MODULE =================
    [HttpGet]
    public async Task<IActionResult> GetByModule([FromQuery] string module)
    {
        if (string.IsNullOrEmpty(module))
            return BadRequest("Module parameter is required");

        var views = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .Where(v => v.Module == module)
            .OrderBy(v => v.Name)
            .ToListAsync();

        return Ok(views);
    }

    // ================= GET ONE VIEW =================
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var view = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (view == null)
            return NotFound();

        return Ok(view);
    }

    // ================= GET DEFAULT VIEW FOR MODULE =================
    [HttpGet("default")]
    public async Task<IActionResult> GetDefault([FromQuery] string module)
    {
        if (string.IsNullOrEmpty(module))
            return BadRequest("Module parameter is required");

        var defaultView = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Module == module && v.IsDefault);

        if (defaultView == null)
        {
            // Create a default view if none exists
            defaultView = await CreateDefaultView(module);
        }

        return Ok(defaultView);
    }

    // ================= CREATE VIEW =================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ViewDefinitionDto dto)
    {
        // If this is marked as default, unset other defaults for this module
        if (dto.IsDefault)
        {
            await UnsetOtherDefaults(dto.Module);
        }

        var view = new ViewDefinition
        {
            Module = dto.Module,
            Name = dto.Name,
            IsDefault = dto.IsDefault,
            UserId = dto.UserId
        };

        _context.ViewDefinitions.Add(view);
        await _context.SaveChangesAsync();

        // Add columns
        if (dto.Columns != null)
        {
            foreach (var colDto in dto.Columns)
            {
                var column = new ViewColumn
                {
                    ViewDefinitionId = view.Id,
                    FieldName = colDto.FieldName,
                    Label = colDto.Label,
                    IsVisible = colDto.IsVisible,
                    Width = colDto.Width,
                    Order = colDto.Order,
                    IsCustomField = colDto.IsCustomField
                };
                _context.ViewColumns.Add(column);
            }
        }

        // Add filters
        if (dto.Filters != null)
        {
            foreach (var filterDto in dto.Filters)
            {
                var filter = new ViewFilter
                {
                    ViewDefinitionId = view.Id,
                    FieldName = filterDto.FieldName,
                    Operator = filterDto.Operator,
                    Value = filterDto.Value,
                    IsCustomField = filterDto.IsCustomField
                };
                _context.ViewFilters.Add(filter);
            }
        }

        // Add sorting
        if (dto.Sorting != null)
        {
            foreach (var sortDto in dto.Sorting)
            {
                var sort = new ViewSorting
                {
                    ViewDefinitionId = view.Id,
                    FieldName = sortDto.FieldName,
                    Direction = sortDto.Direction,
                    Order = sortDto.Order,
                    IsCustomField = sortDto.IsCustomField
                };
                _context.ViewSorting.Add(sort);
            }
        }

        // Add grouping
        if (dto.Grouping != null)
        {
            foreach (var groupDto in dto.Grouping)
            {
                var group = new ViewGrouping
                {
                    ViewDefinitionId = view.Id,
                    FieldName = groupDto.FieldName,
                    Order = groupDto.Order,
                    IsExpanded = groupDto.IsExpanded,
                    IsCustomField = groupDto.IsCustomField
                };
                _context.ViewGrouping.Add(group);
            }
        }

        await _context.SaveChangesAsync();

        // Reload with includes
        var createdView = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Id == view.Id);

        return Ok(createdView);
    }

    // ================= UPDATE VIEW =================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ViewDefinitionDto dto)
    {
        var view = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (view == null)
            return NotFound();

        // If this is marked as default, unset other defaults for this module
        if (dto.IsDefault && !view.IsDefault)
        {
            await UnsetOtherDefaults(dto.Module, id);
        }

        // Update basic properties
        view.Name = dto.Name;
        view.IsDefault = dto.IsDefault;
        view.UpdatedAt = DateTime.UtcNow;

        // Remove existing columns, filters, sorting, grouping
        _context.ViewColumns.RemoveRange(view.Columns);
        _context.ViewFilters.RemoveRange(view.Filters);
        _context.ViewSorting.RemoveRange(view.Sorting);
        _context.ViewGrouping.RemoveRange(view.Grouping);

        await _context.SaveChangesAsync();

        // Add new columns
        if (dto.Columns != null)
        {
            foreach (var colDto in dto.Columns)
            {
                var column = new ViewColumn
                {
                    ViewDefinitionId = view.Id,
                    FieldName = colDto.FieldName,
                    Label = colDto.Label,
                    IsVisible = colDto.IsVisible,
                    Width = colDto.Width,
                    Order = colDto.Order,
                    IsCustomField = colDto.IsCustomField
                };
                _context.ViewColumns.Add(column);
            }
        }

        // Add new filters
        if (dto.Filters != null)
        {
            foreach (var filterDto in dto.Filters)
            {
                var filter = new ViewFilter
                {
                    ViewDefinitionId = view.Id,
                    FieldName = filterDto.FieldName,
                    Operator = filterDto.Operator,
                    Value = filterDto.Value,
                    IsCustomField = filterDto.IsCustomField
                };
                _context.ViewFilters.Add(filter);
            }
        }

        // Add new sorting
        if (dto.Sorting != null)
        {
            foreach (var sortDto in dto.Sorting)
            {
                var sort = new ViewSorting
                {
                    ViewDefinitionId = view.Id,
                    FieldName = sortDto.FieldName,
                    Direction = sortDto.Direction,
                    Order = sortDto.Order,
                    IsCustomField = sortDto.IsCustomField
                };
                _context.ViewSorting.Add(sort);
            }
        }

        // Add new grouping
        if (dto.Grouping != null)
        {
            foreach (var groupDto in dto.Grouping)
            {
                var group = new ViewGrouping
                {
                    ViewDefinitionId = view.Id,
                    FieldName = groupDto.FieldName,
                    Order = groupDto.Order,
                    IsExpanded = groupDto.IsExpanded,
                    IsCustomField = groupDto.IsCustomField
                };
                _context.ViewGrouping.Add(group);
            }
        }

        await _context.SaveChangesAsync();

        // Reload
        var updatedView = await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Id == view.Id);

        return Ok(updatedView);
    }

    // ================= DELETE VIEW =================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var view = await _context.ViewDefinitions.FindAsync(id);
        if (view == null)
            return NotFound();

        _context.ViewDefinitions.Remove(view);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ================= HELPER METHODS =================

    private async Task UnsetOtherDefaults(string module, int? excludeId = null)
    {
        var otherDefaults = await _context.ViewDefinitions
            .Where(v => v.Module == module && v.IsDefault && v.Id != excludeId)
            .ToListAsync();

        foreach (var view in otherDefaults)
        {
            view.IsDefault = false;
        }

        await _context.SaveChangesAsync();
    }

    private async Task<ViewDefinition> CreateDefaultView(string module)
    {
        var view = new ViewDefinition
        {
            Module = module,
            Name = $"All {module}s",
            IsDefault = true
        };

        _context.ViewDefinitions.Add(view);

        // Add default columns based on module
        var defaultColumns = GetDefaultColumns(module);
        foreach (var (fieldName, label, order) in defaultColumns)
        {
            var column = new ViewColumn
            {
                ViewDefinitionId = view.Id,
                FieldName = fieldName,
                Label = label,
                IsVisible = true,
                Order = order,
                IsCustomField = false
            };
            _context.ViewColumns.Add(column);
        }

        await _context.SaveChangesAsync();

        // Reload with includes
        return await _context.ViewDefinitions
            .Include(v => v.Columns)
            .Include(v => v.Filters)
            .Include(v => v.Sorting)
            .Include(v => v.Grouping)
            .FirstOrDefaultAsync(v => v.Id == view.Id) ?? view;
    }

    private List<(string fieldName, string label, int order)> GetDefaultColumns(string module)
    {
        return module switch
        {
            "Lead" => new List<(string, string, int)>
            {
                ("Id", "ID", 0),
                ("Name", "Name", 1),
                ("Email", "Email", 2),
                ("Phone", "Phone", 3),
                ("Company", "Company", 4),
                ("Source", "Source", 5),
                ("Status", "Status", 6)
            },
            "Opportunity" => new List<(string, string, int)>
            {
                ("Id", "ID", 0),
                ("Name", "Name", 1),
                ("Amount", "Amount", 2),
                ("Stage", "Stage", 3),
                ("CloseDate", "Close Date", 4)
            },
            _ => new List<(string, string, int)>
            {
                ("Id", "ID", 0),
                ("Name", "Name", 1)
            }
        };
    }
}

// ================= DTOs =================

public class ViewDefinitionDto
{
    public required string Module { get; set; }
    public required string Name { get; set; }
    public bool IsDefault { get; set; }
    public int? UserId { get; set; }
    public List<ViewColumnDto>? Columns { get; set; }
    public List<ViewFilterDto>? Filters { get; set; }
    public List<ViewSortingDto>? Sorting { get; set; }
    public List<ViewGroupingDto>? Grouping { get; set; }
}

public class ViewColumnDto
{
    public required string FieldName { get; set; }
    public required string Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public int? Width { get; set; }
    public int Order { get; set; }
    public bool IsCustomField { get; set; }
}

public class ViewFilterDto
{
    public required string FieldName { get; set; }
    public required string Operator { get; set; }
    public string? Value { get; set; }
    public bool IsCustomField { get; set; }
}

public class ViewSortingDto
{
    public required string FieldName { get; set; }
    public required string Direction { get; set; }
    public int Order { get; set; }
    public bool IsCustomField { get; set; }
}

public class ViewGroupingDto
{
    public required string FieldName { get; set; }
    public int Order { get; set; }
    public bool IsExpanded { get; set; } = true;
    public bool IsCustomField { get; set; }
}