using BackendApi.Data;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportTicketsController : ControllerBase
{
    private readonly AppDbContext _db;
    public SupportTicketsController(AppDbContext db) => _db = db;

    /// <summary>Lista todos los tickets</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupportTicket>>> GetAll()
        => await _db.SupportTickets.AsNoTracking().ToListAsync();

    /// <summary>Obtiene un ticket por Id</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupportTicket>> GetById(int id)
    {
        var ticket = await _db.SupportTickets.FindAsync(id);
        return ticket is null ? NotFound() : ticket;
    }

    /// <summary>Crea un nuevo ticket</summary>
    [HttpPost]
    public async Task<ActionResult<SupportTicket>> Create(SupportTicket input)
    {
        input.OpenedAt = DateTime.UtcNow;
        if ((input.Status == TicketStatus.Resolved || input.Status == TicketStatus.Closed) && input.ClosedAt is null)
            input.ClosedAt = DateTime.UtcNow;

        _db.SupportTickets.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    /// <summary>Actualiza completamente un ticket</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SupportTicket input)
    {
        if (id != input.Id) return BadRequest();

        var exists = await _db.SupportTickets.AnyAsync(t => t.Id == id);
        if (!exists) return NotFound();

        if ((input.Status == TicketStatus.Resolved || input.Status == TicketStatus.Closed) && input.ClosedAt is null)
            input.ClosedAt = DateTime.UtcNow;
        if (input.Status == TicketStatus.Open || input.Status == TicketStatus.InProgress)
            input.ClosedAt = null;

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Elimina un ticket</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.SupportTickets.FindAsync(id);
        if (entity is null) return NotFound();

        _db.SupportTickets.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}