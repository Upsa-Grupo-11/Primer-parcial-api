using BackendApi.Data;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;
    public EventsController(AppDbContext db) => _db = db;

    /// <summary>Lista todos los eventos</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        => await _db.Events.AsNoTracking().ToListAsync();

    /// <summary>Obtiene un evento por Id</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> GetById(int id)
    {
        var entity = await _db.Events.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    /// <summary>Crea un nuevo evento</summary>
    [HttpPost]
    public async Task<ActionResult<Event>> Create(Event input)
    {
        input.Id = 0; // evitar "explicit value for identity" si alguien manda Id
        _db.Events.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    /// <summary>Actualiza por completo un evento</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Event input)
    {
        if (id != input.Id) return BadRequest();

        var exists = await _db.Events.AnyAsync(e => e.Id == id);
        if (!exists) return NotFound();

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Elimina un evento</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Events.FindAsync(id);
        if (entity is null) return NotFound();

        _db.Events.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}