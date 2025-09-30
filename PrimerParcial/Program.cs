using System.Linq;
using Microsoft.EntityFrameworkCore;
using BackendApi.Data;  // <— tu namespace del DbContext

var builder = WebApplication.CreateBuilder(args);

// ===== DbContext con SQL Server =====
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== Controllers =====
builder.Services.AddControllers();

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BackendApi", Version = "v1" });
});

var app = builder.Build();

// ===== Crear/actualizar BD al arrancar (Azure friendly) =====
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Si hay migraciones pendientes, aplícalas; si no hay, al menos crea la BD.
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();             // aplica migraciones
            logger.LogInformation("✅ Migraciones aplicadas.");
        }
        else
        {
            db.Database.EnsureCreated();       // crea el schema actual (sin migraciones)
            logger.LogInformation("✅ Base creada/confirmada con EnsureCreated.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error inicializando la base de datos.");
        // Si prefieres fallar el arranque en Azure:
        // throw;
    }
}

// ===== Middleware =====
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendApi v1");
    o.RoutePrefix = "swagger"; // disponible en /swagger
});

// app.UseHttpsRedirection(); // opcional en App Service, puedes activarlo
app.UseAuthorization();

app.MapGet("/ping", () => Results.Ok("pong"));
app.MapControllers();

app.Run();