using BackendApi.Data;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    /// <summary>Lista todos los productos</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        => await _db.Products.AsNoTracking().ToListAsync();

    /// <summary>Obtiene un producto por Id</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var entity = await _db.Products.FindAsync(id);
        return entity is null ? NotFound() : entity;
    }

    /// <summary>Crea un producto</summary>
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product input)
    {
        input.CreatedAt = DateTime.UtcNow;
        _db.Products.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    /// <summary>Actualiza un producto</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product input)
    {
        if (id != input.Id) return BadRequest();
        var exists = await _db.Products.AnyAsync(p => p.Id == id);
        if (!exists) return NotFound();

        input.UpdatedAt = DateTime.UtcNow;
        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Elimina un producto</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}