using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Event> Events => Set<Event>();   // <- añade esta línea si no la tienes
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
}